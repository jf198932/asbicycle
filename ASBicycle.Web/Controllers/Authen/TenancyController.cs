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

                //var session = Session["currentUser"] as BackLoginModel;
                //var backuser = _backUserRepository.Insert(new BackUser
                //{
                //    LoginName = "admin",
                //    LoginPwd = "8wdJLK8mokI=",
                //    FullName = "admin",
                //    Enabled = true,
                //    School_id = school.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //CurrentUnitOfWork.SaveChanges();
                //var role = _roleRepository.Insert(new Role
                //{
                //    Name = "系统管理员",
                //    Description = "开发人员、系统配置人员使用",
                //    OrderSort = 1,
                //    Enabled = true,
                //    School_id = school.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //CurrentUnitOfWork.SaveChanges();
                //_userRoleRepository.Insert(new UserRole {UserId = backuser.Id, RoleId = role.Id});
                //CurrentUnitOfWork.SaveChanges();

                //var m1 = _moduleRepository.Insert(new Module
                //{
                //    ParentId = null,
                //    Name = "权限管理",
                //    LinkUrl = null,
                //    Area = null,
                //    Controller = null,
                //    Action = null,
                //    Icon = "fa fa-cloud",
                //    Code = "",
                //    OrderSort = 1,
                //    Description = null,
                //    IsMenu = true,
                //    Enabled = true,
                //    School_id = school.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //CurrentUnitOfWork.SaveChanges();
                //var m2 = _moduleRepository.Insert(new Module
                //{
                //    ParentId = m1.Id,
                //    Name = "角色管理",
                //    LinkUrl = "Role/List",
                //    Area = "",
                //    Controller = "Role",
                //    Action = "List",
                //    Icon = "",
                //    Code = "",
                //    OrderSort = 2,
                //    Description = null,
                //    IsMenu = true,
                //    Enabled = true,
                //    School_id = school.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //var m3 = _moduleRepository.Insert(new Module
                //{
                //    ParentId = m1.Id,
                //    Name = "用户管理",
                //    LinkUrl = "BackUser/List",
                //    Area = "",
                //    Controller = "User",
                //    Action = "List",
                //    Icon = "",
                //    Code = "",
                //    OrderSort = 3,
                //    Description = null,
                //    IsMenu = true,
                //    Enabled = true,
                //    School_id = school.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //var m4 = _moduleRepository.Insert(new Module
                //{
                //    ParentId = m1.Id,
                //    Name = "模块管理",
                //    LinkUrl = "Module/List",
                //    Area = "",
                //    Controller = "Module",
                //    Action = "List",
                //    Icon = "",
                //    Code = "",
                //    OrderSort = 4,
                //    Description = null,
                //    IsMenu = true,
                //    Enabled = true,
                //    School_id = school.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //var m5 = _moduleRepository.Insert(new Module
                //{
                //    ParentId = m1.Id,
                //    Name = "权限管理",
                //    LinkUrl = "Permission/List",
                //    Area = "",
                //    Controller = "Permission",
                //    Action = "List",
                //    Icon = "",
                //    Code = "",
                //    OrderSort = 5,
                //    Description = null,
                //    IsMenu = true,
                //    Enabled = true,
                //    School_id = school.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //CurrentUnitOfWork.SaveChanges();

                //var p1 = _permissionRepository.Insert(new Permission
                //{
                //    Code = "Index",
                //    Name = "浏览",
                //    OrderSort = 1,
                //    Icon = null,
                //    Description = null,
                //    Enabled = true,
                //    School_id = school.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //var p2 = _permissionRepository.Insert(new Permission
                //{
                //    Code = "Create",
                //    Name = "新增",
                //    OrderSort = 2,
                //    Icon = "icon-plus",
                //    Description = null,
                //    Enabled = true,
                //    School_id = school.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //var p3 = _permissionRepository.Insert(new Permission
                //{
                //    Code = "Edit",
                //    Name = "编辑",
                //    OrderSort = 3,
                //    Icon = "icon-pencil",
                //    Description = null,
                //    Enabled = true,
                //    School_id = school.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //var p4 = _permissionRepository.Insert(new Permission
                //{
                //    Code = "Delete",
                //    Name = "删除",
                //    OrderSort = 4,
                //    Icon = "icon-remove",
                //    Description = null,
                //    Enabled = true,
                //    School_id = school.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //var p5 = _permissionRepository.Insert(new Permission
                //{
                //    Code = "SetButton",
                //    Name = "设置按钮",
                //    OrderSort = 5,
                //    Icon = "icon-legal",
                //    Description = null,
                //    Enabled = true,
                //    School_id = school.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //var p6 = _permissionRepository.Insert(new Permission
                //{
                //    Code = "SetPermission",
                //    Name = "设置权限",
                //    OrderSort = 6,
                //    Icon = "icon-sitemap",
                //    Description = null,
                //    Enabled = true,
                //    School_id = school.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //CurrentUnitOfWork.SaveChanges();

                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m2.Id,
                //    PermissionId = p1.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m2.Id,
                //    PermissionId = p2.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m2.Id,
                //    PermissionId = p3.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m2.Id,
                //    PermissionId = p4.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m2.Id,
                //    PermissionId = p6.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});

                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m3.Id,
                //    PermissionId = p1.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m3.Id,
                //    PermissionId = p2.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m3.Id,
                //    PermissionId = p3.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m3.Id,
                //    PermissionId = p4.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});

                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m4.Id,
                //    PermissionId = p1.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m4.Id,
                //    PermissionId = p2.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m4.Id,
                //    PermissionId = p3.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m4.Id,
                //    PermissionId = p4.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m4.Id,
                //    PermissionId = p5.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});

                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m5.Id,
                //    PermissionId = p1.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m5.Id,
                //    PermissionId = p2.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m5.Id,
                //    PermissionId = p3.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_modulePermissionRepository.Insert(new ModulePermission
                //{
                //    ModuleId = m5.Id,
                //    PermissionId = p4.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //CurrentUnitOfWork.SaveChanges();

                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m1.Id,
                //    PermissionId = null,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m2.Id,
                //    PermissionId = p1.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m2.Id,
                //    PermissionId = p2.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m2.Id,
                //    PermissionId = p3.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m2.Id,
                //    PermissionId = p4.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m2.Id,
                //    PermissionId = p6.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});

                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m3.Id,
                //    PermissionId = p1.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m3.Id,
                //    PermissionId = p2.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m3.Id,
                //    PermissionId = p3.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m3.Id,
                //    PermissionId = p4.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});

                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m4.Id,
                //    PermissionId = p1.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m4.Id,
                //    PermissionId = p2.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m4.Id,
                //    PermissionId = p3.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m4.Id,
                //    PermissionId = p4.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m4.Id,
                //    PermissionId = p5.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});

                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m5.Id,
                //    PermissionId = p1.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m5.Id,
                //    PermissionId = p2.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m5.Id,
                //    PermissionId = p3.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //_roleModulePermissionRepository.Insert(new RoleModulePermission
                //{
                //    RoleId = role.Id,
                //    ModuleId = m5.Id,
                //    PermissionId = p4.Id,
                //    CreateBy = session.LoginName,
                //    CreateId = session.Id,
                //    CreateTime = DateTime.Now,
                //    ModifyId = session.Id,
                //    ModifyBy = session.LoginName,
                //    ModifyTime = DateTime.Now
                //});
                //CurrentUnitOfWork.SaveChanges();
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