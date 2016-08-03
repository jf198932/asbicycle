using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Web.Models;
using ASBicycle.Entities.Authen;
using ASBicycle.Web.Extension.Fliter;
using ASBicycle.Web.Models.Authen;
using ASBicycle.Web.Models.Common;
using AutoMapper;

namespace ASBicycle.Web.Controllers.Authen
{
    public class ModuleController : ASBicycleControllerBase
    {
        private readonly IRepository<Module> _moduleRepository;
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IRepository<ModulePermission> _modulePermissionRepository; 

        public ModuleController(IRepository<Module> moduleRepository, IRepository<Permission> permissionRepository, IRepository<ModulePermission> modulePermissionRepository)
        {
            _moduleRepository = moduleRepository;
            _permissionRepository = permissionRepository;
            _modulePermissionRepository = modulePermissionRepository;
        }

        // GET: Module
        
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        [AdminLayout]
        //[AdminPermission(PermissionCustomMode.Enforce)]
        public ActionResult List()
        {
            return View();
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {

            var query =
                _moduleRepository.GetAll().OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = _moduleRepository.Count();
            var filterResult = query.Select(t => new ModuleModel
            {
                Id = t.Id,
                Name = "<i class='" + t.Icon + "'></i> " + t.Name,
                Code = t.Code,
                Icon = t.Icon,
                ParentId = t.ParentId,
                ParentName = t.ParentModule != null ? t.ParentModule.Name : "",
                LinkUrl = t.LinkUrl,
                OrderSort = t.OrderSort,
                IsMenu = t.IsMenu,
                Enabled = t.Enabled,
                Area = t.Area,
                Controller = t.Controller,
                Action = t.Action
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.Name,
                                t.Code,
                                t.ParentName,
                                t.LinkUrl,
                                t.OrderSort.ToString(),
                                t.IsMenu ? "1":"0",
                                t.Enabled ? "1":"0",
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        public ActionResult Create()
        {
            var model = new ModuleModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Create(ModuleModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<ModuleModel, Module>();
                var user = Mapper.Map<Module>(model);
                if (!string.IsNullOrEmpty(model.LinkUrl))
                {
                    string[] link = model.LinkUrl.Split('/');
                    if (link.Length > 2)
                    {
                        user.Area = link[0];
                        user.Controller = link[1];
                        user.Action = link[2];
                    }
                    else
                    {
                        user.Controller = link[0];
                        user.Action = link[1];
                    }
                }
                _moduleRepository.Insert(user);
                //SuccessNotification("添加成功");
                return Json(model);
            }
            return Json(null);
        }

        [UnitOfWork]
        public virtual ActionResult SetButton(int id)
        {
            var module = _moduleRepository.Get(id);
            var model = new ButtonModel();
            if (module == null)
            {
                return PartialView(model);
            }
            model.ModuleId = module.Id;
            model.ModuleName = module.Name;
            PrepareAllUserModel(model);
            foreach (var item in module.ModulePermission)
            {
                model.SelectedButtonList.Add(item.PermissionId);
            }
            return PartialView(model);
        }
        [HttpPost, UnitOfWork]
        public virtual ActionResult SetButton(ButtonModel model)
        {
            if (ModelState.IsValid)
            {
                //删除重复
                _modulePermissionRepository.Delete(mp=> mp.ModuleId == model.ModuleId && !model.SelectedButtonList.Contains(mp.PermissionId));
                var modulePermissionList =
                    _modulePermissionRepository.GetAll().Where(mp => mp.ModuleId == model.ModuleId).ToList();
                //List<ModulePermission> setMP = new List<ModulePermission>();
                foreach (var permissionId in model.SelectedButtonList)
                {
                    if (modulePermissionList.All(ur => ur.PermissionId != permissionId))
                    {
                        //setMP.Add(new ModulePermission
                        //{
                        //    ModuleId = model.ModuleId,
                        //    PermissionId = permissionId
                        //});
                        _modulePermissionRepository.Insert(new ModulePermission
                        {
                            ModuleId = model.ModuleId,
                            PermissionId = permissionId
                        });
                    }
                }
                //_modulePermissionRepository.Insert(new ModulePermission());
                //_unitOfWorkManager.Current.SaveChanges();
                return PartialView(model);
                
            }
            else
            {
                return PartialView(model);
            }
        }

        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.CreateMap<Module, ModuleModel>();
            var entity = _moduleRepository.Get(id);
            var model = Mapper.Map<ModuleModel>(entity);
            //var model = role.ToModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Edit(ModuleModel model)
        {
            if (ModelState.IsValid)
            {
                var module = _moduleRepository.FirstOrDefault(m => m.Id == model.Id);
                module.Icon = model.Icon;
                module.IsMenu = model.IsMenu;
                module.LinkUrl = model.LinkUrl;
                module.OrderSort = model.OrderSort;
                module.ParentId = model.ParentId;
                module.Code = model.Code;
                module.Enabled = model.Enabled;
                module.Name = model.Name;
                module.Description = model.Description;
                if (!string.IsNullOrEmpty(model.LinkUrl))
                {
                    string[] link = model.LinkUrl.Split('/');
                    if (link.Length > 2)
                    {
                        module.Area = link[0];
                        module.Controller = link[1];
                        module.Action = link[2];
                    }
                    else
                    {
                        module.Controller = link[0];
                        module.Action = link[1];
                    }
                }

                _moduleRepository.InsertOrUpdate(module);
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
            _moduleRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        #region 公共方法
        [NonAction, UnitOfWork]
        protected virtual void PrepareAllUserModel(ModuleModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            model.ParentModuleItems.AddRange(
                _moduleRepository.GetAll()
                    .Where(m => m.Enabled && m.IsMenu)
                    .OrderBy(m => m.OrderSort)
                    .Select(m => new SelectListItem {Text = m.Name, Value = m.Id.ToString()}));
            model.ParentModuleItems.Insert(0, new SelectListItem {Text = "--根模块--", Value = ""});
            //model.RoleList.Add(
            //    _schoolRepository.GetAll().Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() }));

        }

        [NonAction, UnitOfWork]
        protected virtual void PrepareAllUserModel(ButtonModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            model.ButtonList =
                _permissionRepository.GetAll()
                    .Where(r => r.Enabled)
                    .OrderBy(r => r.OrderSort)
                    .Select(r => new KeyValueModel { Text = r.Name, Value = r.Id.ToString() })
                    .ToList();

            //model.RoleList.Add(
            //    _schoolRepository.GetAll().Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() }));

        }

        #endregion
    }
}