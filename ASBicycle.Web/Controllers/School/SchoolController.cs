using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Abp.Web.Models;
using ASBicycle.School;
using ASBicycle.User.Dto;
using ASBicycle.Web.Extension.Fliter;
using ASBicycle.Web.Helper;
using ASBicycle.Web.Models.Common;
using ASBicycle.Web.Models.School;
using AutoMapper;

namespace ASBicycle.Web.Controllers.School
{
    public class SchoolController : ASBicycleControllerBase
    {
        private readonly ISchoolRepository _schoolRepository;

        public SchoolController(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        // GET: School
        //[AdminLayout]
        public ActionResult Index()
        {
            return RedirectToAction("Info");
        }
        [AdminLayout, UnitOfWork]
        //[AdminPermission(PermissionCustomMode.Enforce)]
        public ActionResult Info()
        {
            var id = CommonHelper.GetSchoolId();
            Mapper.CreateMap<Entities.School, SchoolModel>();
            var model = Mapper.Map<SchoolModel>(_schoolRepository.FirstOrDefault(t=>t.Id == id));
            return View(model);
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

                school = _schoolRepository.Update(school);
                //role = model.ToEntity(role);
                //_roleService.UpdateRole(role);

                //SuccessNotification("更新成功");
                return Json(model);
            }
            return Json(null);
        }
    }
}