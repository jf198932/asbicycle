using System;
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
using Abp.Extensions;

namespace ASBicycle.Bike
{
    public class BikeAppService : ASBicycleAppServiceBase, IBikeAppService
    {
        private readonly IBikeRepository _bikeRepository;

        public BikeAppService(IBikeRepository bikeRepository)
        {
            _bikeRepository = bikeRepository;
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