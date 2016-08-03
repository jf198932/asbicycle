using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Abp.Web.Models;
using ASBicycle.School;
using ASBicycle.User;
using ASBicycle.Web.Extension.Fliter;
using ASBicycle.Web.Models.Common;
using ASBicycle.Web.Models.School;
using AutoMapper;

namespace ASBicycle.Web.Controllers.School
{
    public class UserController : ASBicycleControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ISchoolRepository _schoolRepository;

        public UserController(IUserRepository userRepository, ISchoolRepository schoolRepository)
        {
            _userRepository = userRepository;
            _schoolRepository = schoolRepository;
        }
        // GET: User
        //[AdminLayout]
        public ActionResult Index()
        {
            return RedirectToAction("ListU");
        }
        [AdminLayout]
        [AdminPermission(PermissionCustomMode.Enforce)]
        public ActionResult ListU()
        {
            return View("List");
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {

            var query =
                _userRepository.GetAll().OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = _userRepository.Count();
            var filterResult = query.Select(t => new UserModel
            {
                Id = t.Id,
                Name = t.Name,
                Phone = t.Phone,
                Nickname = t.Nickname,
                Weixacc = t.Weixacc,
                School_id = t.School_id,
                Email = t.Email,
                Certification = t.Certification,
                Textmsg = t.Textmsg,
                Textmsg_time = DateTime.Now,
                Remember_token = t.Remember_token,
                Credits = t.Credits,
                Balance = t.Balance,
                School_name = t.School.Name
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.Phone,
                                t.Name,
                                t.Nickname,
                                t.School_name,
                                t.Credits.ToString(),
                                t.Balance.ToString(),
                                t.Certification.ToString(),
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        public ActionResult Create()
        {
            var model = new UserModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Create(UserModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<UserModel, Entities.User>();
                var user = Mapper.Map<Entities.User>(model);
                user = _userRepository.Insert(user);

                //SuccessNotification("添加成功");
                return Json(model);
            }
            return Json(null);
        }
        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.CreateMap<Entities.User, UserModel>();
            var model = Mapper.Map<UserModel>(_userRepository.Get(id));
            //var model = role.ToModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Edit(UserModel model)
        {
            var user = _userRepository.Get(model.Id);

            if (ModelState.IsValid)
            {
                user.Name = model.Name;
                user.Nickname = model.Nickname;
                user.School_id = model.School_id;
                user.Certification = model.Certification;
                user.Credits = model.Credits;
                user.Balance = model.Balance;
                user.Updated_at = DateTime.Now;

                user = _userRepository.Update(user);
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
            _userRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        #region 公共方法
        [NonAction, UnitOfWork]
        protected virtual void PrepareAllUserModel(UserModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            model.SchoolList.AddRange(
                _schoolRepository.GetAll().Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() }));
            model.CertificationList.AddRange(new List<SelectListItem>
            {
                new SelectListItem {Text = "未申请", Value = "1"},
                new SelectListItem {Text = "已申请", Value = "2"},
                new SelectListItem {Text = "已认证", Value = "3"},
                new SelectListItem {Text = "认证失败", Value = "4"}
            });
        }

        #endregion
    }
}