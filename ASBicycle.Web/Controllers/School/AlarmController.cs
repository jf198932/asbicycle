using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Web.Models;
using ASBicycle.Web.Helper;
using ASBicycle.Web.Models.Common;
using ASBicycle.Web.Models.School;

namespace ASBicycle.Web.Controllers.School
{
    public class AlarmController : ASBicycleControllerBase
    {
        private readonly IRepository<Entities.Bike> _bikeRepository;

        public AlarmController(IRepository<Entities.Bike> bikeRepository)
        {
            _bikeRepository = bikeRepository;
        }
        // GET: Alarm
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            BikeModel model = new BikeModel();
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _bikeRepository.GetAll().Where(expr);
            var query = temp
                    .OrderBy(s => s.Id)
                    .Skip(param.iDisplayStart)
                    .Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.Select(t => new BikeModel
            {
                Id = t.Id,
                Ble_name = t.Ble_name,
                Ble_serial = t.Ble_serial,
                Ble_type = t.Ble_type,
                Lock_status = t.Lock_status,
                Bike_status = t.Bike_status,
                Vlock_status = t.Vlock_status,
                Position = t.Position,
                Battery = t.Battery,
                User_id = t.User_id,
                Bike_img = t.Bike_img,
                Bikesite_id = t.Bikesite_id,
                Insite_status = t.Bikesite_id == null ? 2 : 1,
                Bikesite_name = t.Bikesite == null ? "" : t.Bikesite.Name,
                User_name = t.User == null ? "" : t.User.Name,
                Phone = t.User == null ? "" : t.User.Phone
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.Ble_serial,
                                t.Ble_name,
                                t.Ble_type.ToString(),
                                t.Vlock_status.ToString(),
                                t.Bikesite_name,
                                t.Phone,
                                t.Position,
                                t.Battery.ToString(),
                                t.Insite_status.ToString(),
                                t.Bike_img,
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        #region 构建查询表达式
        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <returns></returns>
        private Expression<Func<Entities.Bike, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<Entities.Bike> bulider = new DynamicLambda<Entities.Bike>();
            Expression<Func<Entities.Bike, Boolean>> expr = null;
            if (!string.IsNullOrEmpty(Request["Ble_serial"]))
            {
                var data = Request["Ble_serial"].Trim();
                Expression<Func<Entities.Bike, Boolean>> tmp = t => t.Ble_serial.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Ble_name"]))
            {
                var data = Request["Ble_name"].Trim();
                Expression<Func<Entities.Bike, Boolean>> tmp = t => t.Ble_name.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Ble_type"]) && Request["Ble_type"].Trim() != "0")
            {
                var data = Convert.ToInt32(Request["Ble_type"].Trim());
                Expression<Func<Entities.Bike, Boolean>> tmp = t => t.Ble_type >= data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            
            //Expression<Func<Entities.Bike, Boolean>> tmpd = t => t.Vlock_status >= 3;
            //expr = bulider.BuildQueryAnd(expr, tmpd);
            
            var id = CommonHelper.GetSchoolId();
            Expression<Func<Entities.Bike, Boolean>> tmpSolid = t => t.School_id == id;
            expr = bulider.BuildQueryAnd(expr, tmpSolid);

            return expr;
        }

        #endregion
    }
}