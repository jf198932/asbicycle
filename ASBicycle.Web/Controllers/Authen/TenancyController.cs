using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Web.Models;
using ASBicycle.Entities.Authen;
using ASBicycle.School;
using ASBicycle.Web.Extension.Fliter;
using ASBicycle.Web.Models.Common;
using ASBicycle.Web.Models.School;
using AutoMapper;

namespace ASBicycle.Web.Controllers.Authen
{
    public class TenancyController : ASBicycleControllerBase
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Module> _moduleRepository;
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IRepository<BackUser> _backUserRepository;
        private readonly IRepository<RoleModulePermission> _roleModulePermissionRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<ModulePermission> _modulePermissionRepository;


        public TenancyController(ISchoolRepository schoolRepository,
            IRepository<Role> roleRepository, IRepository<Module> moduleRepository, IRepository<Permission> permissionRepository, IRepository<BackUser> backUserRepository,
            IRepository<RoleModulePermission> roleModulePermissionRepository, IRepository<UserRole> userRoleRepository, IRepository<ModulePermission> modulePermissionRepository)
        {
            _schoolRepository = schoolRepository;
            _roleRepository = roleRepository;
            _moduleRepository = moduleRepository;
            _permissionRepository = permissionRepository;
            _backUserRepository = backUserRepository;
            _roleModulePermissionRepository = roleModulePermissionRepository;
            _userRoleRepository = userRoleRepository;
            _modulePermissionRepository = modulePermissionRepository;
        }
        // GET: Tenancy
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
                _schoolRepository.GetAll().OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = _schoolRepository.Count();
            var filterResult = query.Select(t => new SchoolModel
            {
                Id = t.Id,
                Name = t.Name,
                Areacode = t.Areacode,
                Gps_point = t.Gps_point,
                Site_count = t.Site_count,
                Bike_count = t.Bike_count,
                Time_charge = t.Time_charge,
                TenancyName = t.TenancyName

            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.TenancyName,
                                t.Name,
                                t.Areacode,
                                t.Gps_point,
                                t.Site_count.ToString(),
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        public ActionResult Create()
        {
            var model = new SchoolModel();
            return PartialView(model);
        }

        [HttpPost, UnitOfWork, DontWrapResult]
        public virtual ActionResult Create(SchoolModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<SchoolModel, Entities.School>();
                var school = Mapper.Map<Entities.School>(model);
                _schoolRepository.Insert(school);
                CurrentUnitOfWork.SaveChanges();
                
                return Json(model);
            }
            return Json(null);
        }
        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.CreateMap<Entities.School, SchoolModel>();
            var model = Mapper.Map<SchoolModel>(_schoolRepository.Get(id));
            //var model = role.ToModel();

            return PartialView(model);
        }

        [HttpPost, UnitOfWork, DontWrapResult]
        public virtual ActionResult Edit(SchoolModel model)
        {
            var school = _schoolRepository.Get(model.Id);

            if (ModelState.IsValid)
            {
                school.Name = model.Name;
                school.Areacode = model.Areacode;
                school.Bike_count = model.Bike_count;
                school.Gps_point = model.Gps_point;
                school.Site_count = model.Site_count;
                school.Time_charge = model.Time_charge;
                school.Refresh_date = DateTime.Now;
                school.Updated_at = DateTime.Now;

                _schoolRepository.Update(school);
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
            _schoolRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        [UnitOfWork, DontWrapResult]
        public virtual ActionResult CheckTenancyNameExists(string tenancyName)
        {
            var model = _schoolRepository.FirstOrDefault(t => t.TenancyName.ToLower() == tenancyName.ToLower());
            if (model != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}