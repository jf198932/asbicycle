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
    public class PermissionController : ASBicycleControllerBase
    {
        private readonly IRepository<Permission> _permissionRepository;

        public PermissionController(IRepository<Permission> permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        // GET: Permission
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
                _permissionRepository.GetAll().OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = _permissionRepository.Count();
            var filterResult = query.Select(t => new PermissionModel
            {
                Id = t.Id,
                Code = t.Code,
                Name = t.Name,
                Description = t.Description,
                OrderSort = t.OrderSort,
                Icon = t.Icon,
                Enabled = t.Enabled
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.Name,
                                t.Code,
                                t.Icon,
                                t.OrderSort.ToString(),
                                t.Description,
                                t.Enabled ? "1":"0",
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        public ActionResult Create()
        {
            var model = new PermissionModel();
            //PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Create(PermissionModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<PermissionModel, Permission>();
                var user = Mapper.Map<Permission>(model);
                user = _permissionRepository.Insert(user);

                //SuccessNotification("添加成功");
                return Json(model);
            }
            return Json(null);
        }
        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.CreateMap<Permission, PermissionModel>();
            var model = Mapper.Map<PermissionModel>(_permissionRepository.Get(id));
            //var model = role.ToModel();
            //PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Edit(PermissionModel model)
        {
            var user = _permissionRepository.Get(model.Id);

            if (ModelState.IsValid)
            {
                user.Name = model.Name;
                user.Code = model.Code;
                user.Icon = model.Icon;
                user.Description = model.Description;
                user.OrderSort = model.OrderSort;
                user.Enabled = model.Enabled;

                _permissionRepository.Update(user);
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
            _permissionRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }
    }
}