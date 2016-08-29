using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Abp.AutoMapper;
using Abp.UI;
using ASBicycle.Bike;
using ASBicycle.Bike.Dto;
using System.Drawing;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Extensions;
using ASBicycle.Entities;
using AutoMapper;

namespace ASBicycle.Bike
{
    public class BikeAppService : ASBicycleAppServiceBase, IBikeAppService
    {
        private readonly IBikeRepository _bikeRepository;
        private readonly ISqlExecuter _sqlExecuter; 

        public BikeAppService(IBikeRepository bikeRepository, ISqlExecuter sqlExecuter)
        {
            _bikeRepository = bikeRepository;
            _sqlExecuter = sqlExecuter;
        }
        [HttpGet]
        public async Task<AlarmBikeOutput> GetAlarmBikeWay([FromUri] string phone)
        {
            var bike = await _bikeRepository.FirstOrDefaultAsync(t => t.User.Phone == phone);
            var model = new AlarmBikeOutput();
            if (bike == null)
            {
                throw new UserFriendlyException("请先进行绑定");
            }
            model.bikename = bike.Ble_name;
            model.bikeimg = bike.Bike_img;

            var sqlstr =
                "select b.gps_point,b.`name` as sitename,DATE_FORMAT(op_time,'%Y-%m-%d %H:%i:%S') as alarmtime,bikesite_id,bike_id from log as l JOIN bikesite as b on l.bikesite_id = b.id WHERE op_time >= (select op_time from log where type = 3 and bike_id = " + bike.Id+" ORDER BY op_time DESC LIMIT 1) and l.bike_id="+bike.Id+" order by op_time desc";
            model.alarmlist = _sqlExecuter.SqlQuery<AlarmBikeDto>(sqlstr).ToList();
            foreach (var item in model.alarmlist)
            {
                if (!item.gps_point.IsNullOrEmpty())
                {
                    var tempgps = item.gps_point.Split(',');
                    item.lon = double.Parse(tempgps[0]);
                    item.lat = double.Parse(tempgps[1]);
                }
            }
            return model;
        }

        public async Task<BikeOutput> GetBikeInfo([FromUri]string serial)
        {
            var model = await _bikeRepository.FirstOrDefaultAsync(b => b.Ble_name == serial);
            if(model == null)
                throw new UserFriendlyException("没有该车辆");
            var result = model.MapTo<BikeOutput>();
            //result.School_name = model.User.School == null ? "":model.User.School.Name;
            result.Bikesite_name = model.Bikesite == null ? "":model.Bikesite.Name;

            return result;
        }
        [HttpPost]
        public async Task UpdateBike(BikeInput bikeInput)
        {
            var bike = await _bikeRepository.FirstOrDefaultAsync(b => b.Ble_name == bikeInput.Serial);
            if(bikeInput.Vlock_status != null)
                bike.Vlock_status = bikeInput.Vlock_status;
            if(bikeInput.Bike_status != null)
                bike.Bike_status = bikeInput.Bike_status;
            bike.Bike_img = bikeInput.Bike_img;
            bike.Battery = bikeInput.Battery;
            if(bikeInput.Bikesite_id != null)
                bike.Bikesite_id = bikeInput.Bikesite_id;
            bike.Position = bikeInput.Position;
            await _bikeRepository.UpdateAsync(bike);
        }
        [HttpPost]
        public async Task<BikeUploadOutput> UploadBikePic()
        {
            //foreach (string f in HttpContext.Current.Request.Files.AllKeys)
            //{
            //var bikeid = HttpContext.Current.Request.Params["bikeid"];

            //if (bikeid.IsNullOrEmpty())
            //    throw new UserFriendlyException("没有接收到bikeid");
            HttpPostedFile file = HttpContext.Current.Request.Files["filedata"];
            if (file != null)
            {
                //var bikemodel = await _bikeRepository.FirstOrDefaultAsync(t => t.Ble_name == bikeid);

                //if (null == bikemodel)
                //    throw new UserFriendlyException("请输入正确的追踪器编号");

                try
                {
                    // 文件上传后的保存路径
                    string filePath = HttpContext.Current.Server.MapPath("~/Uploads/Bike/");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fileName = Path.GetFileName(file.FileName);// 原始文件名称
                    string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                    string saveName = Guid.NewGuid() + fileExtension; // 保存文件名称

                    file.SaveAs(filePath + saveName);

                    

                    var img = ConfigurationManager.AppSettings["ServerPath"] + "Uploads/Bike/" + saveName;
                    //bikemodel.Bike_img = img;
                    //await _bikeRepository.UpdateAsync(bikemodel);
                    var result = new BikeUploadOutput
                    {
                        ImgUrl = img
                    };
                    return result;
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message);

                }
            }
            else
            {
                throw new UserFriendlyException("请选择一张图片上传");
            }
            //}
        }
    }
}