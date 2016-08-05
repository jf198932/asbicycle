using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Security;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Web.Models;
using Abp.WebApi.Authorization;
using ASBicycle.Entities.Authen;
using ASBicycle.Web.Extension.Fliter;
using ASBicycle.Web.Helper;
using ASBicycle.Web.Models.Authen;
using ASBicycle.Web.Models.Common;
using AutoMapper;

namespace ASBicycle.Web.Controllers.Authen
{
    public class BackUserController : ASBicycleControllerBase
    {
        private readonly IRepository<BackUser> _backUserRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole> _userRoleRepository;

        public BackUserController(IRepository<BackUser> backUserrepository, IRepository<Role> roleRepository, IRepository<UserRole> userRoleRepository)
        {
            _backUserRepository = backUserrepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }
        // GET: BackUser
        //[AdminLayout]
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        [AdminLayout]
        //[AdminPermission(PermissionCustomMode.Enforce)]
        public ActionResult List()
        {
            var model = new BackUserModel();
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _backUserRepository.GetAll().Where(expr);

            var query =
                temp.OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.Select(t => new BackUserModel
            {
                Id = t.Id,
                LoginName = t.LoginName,
                FullName = t.FullName,
                Phone = t.Phone,
                Email = t.Email,
                Enabled = t.Enabled,
                PwdErrorCount = t.PwdErrorCount,
                LoginCount = t.LoginCount,
                RegisterTime = t.RegisterTime,
                LastLoginTime = t.LastLoginTime
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.LoginName,
                                t.FullName,
                                t.Phone,
                                t.Email,
                                t.Enabled ? "1":"0",
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        public ActionResult Create()
        {
            var model = new BackUserModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Create(BackUserModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<BackUserModel, BackUser>();
                var user = Mapper.Map<BackUser>(model);
                user.LoginPwd = DESProvider.EncryptString("123456");//初始密码
                foreach (var roleId in model.SelectedRoleList)
                {
                    user.UserRole.Add(new UserRole
                    {
                        BackUser = user,
                        RoleId = roleId,
                        UserId = user.Id
                    });
                }

                _backUserRepository.Insert(user);
                //SuccessNotification("添加成功");
                return Json(model);
            }
            return Json(null);
        }
        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.CreateMap<BackUser, BackUserModel>();
            var entity = _backUserRepository.Get(id);
            var model = Mapper.Map<BackUserModel>(entity);
            //var model = role.ToModel();
            PrepareAllUserModel(model);
            foreach (var item in entity.UserRole)
            {
                model.SelectedRoleList.Add(item.RoleId);
            }
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Edit(BackUserModel model)
        {
            

            if (ModelState.IsValid)
            {
                //删除重复
                _userRoleRepository.Delete(ur => ur.UserId == model.Id && !model.SelectedRoleList.Contains(ur.RoleId));
                var user = _backUserRepository.Get(model.Id);
                var userrole = user.UserRole.ToList();
                user.FullName = model.FullName;
                user.Phone = model.Phone;
                user.Email = model.Email;
                user.Enabled = model.Enabled;

                //var oldRoleIds = user.UserRole.Select(t => t.RoleId).ToList();
                //var newRoleIds = model.SelectedRoleList.ToList();
                //var intersectRoleIds = oldRoleIds.Intersect(newRoleIds).ToList(); // Same Ids
                //var removeIds = oldRoleIds.Except(intersectRoleIds).ToList(); // Remove Ids
                //var addIds = newRoleIds.Except(intersectRoleIds).ToList(); // Add Ids

                foreach (var roleId in model.SelectedRoleList)
                {
                    if (userrole.All(ur => ur.RoleId != roleId))
                    {
                        user.UserRole.Add(new UserRole
                        {
                            BackUser = user,
                            RoleId = roleId,
                            UserId = user.Id
                        });
                    }
                }

                _backUserRepository.InsertOrUpdate(user);
                //role = model.ToEntity(role);
                //_roleService.UpdateRole(role);

                //SuccessNotification("更新成功");
                return Json(model);
            }
            return Json(null);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Delete(int id)
        {
            _backUserRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        #region 公共方法
        [NonAction, UnitOfWork]
        protected virtual void PrepareAllUserModel(BackUserModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            model.RoleList =
                _roleRepository.GetAll()
                    .Where(r => r.Enabled)
                    .OrderBy(r => r.OrderSort)
                    .Select(r => new KeyValueModel {Text = r.Name, Value = r.Id.ToString()})
                    .ToList();
            //model.RoleList.Add(
            //    _schoolRepository.GetAll().Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() }));

        }

        #region 构建查询表达式
        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <returns></returns>
        private Expression<Func<BackUser, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<BackUser> bulider = new DynamicLambda<BackUser>();
            Expression<Func<BackUser, Boolean>> expr = null;
            if (!string.IsNullOrEmpty(Request["LoginName"]))
            {
                var data = Request["LoginName"].Trim();
                Expression<Func<BackUser, Boolean>> tmp = t => t.LoginName.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["FullName"]))
            {
                var data = Request["FullName"].Trim();
                Expression<Func<BackUser, Boolean>> tmp = t => t.FullName.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Enabled"]) && Request["Enabled"].Trim() != "-1")
            {
                var data = Convert.ToInt32(Request["Enabled"].Trim()) == 1;
                Expression<Func<BackUser, Boolean>> tmp = t => t.Enabled == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            Expression<Func<BackUser, Boolean>> tmpSolid = t => t.School_id == SchoolId;
            expr = bulider.BuildQueryAnd(expr, tmpSolid);

            return expr;
        }

        #endregion
        #endregion
    }
}