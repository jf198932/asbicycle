using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Abp.UI;
using ASBicycle.Bike.Dto;
using ASBicycle.Entities;
using ASBicycle.TbTroubleFeedback.Dto;

namespace ASBicycle.TbTroubleFeedback
{
    public class TroubleAppService : ASBicycleAppServiceBase, ITroubleAppService
    {
        private readonly ITbTroubleFeedbackWriteRepository _tbTroubleFeedbackWriteRepository;

        public TroubleAppService(ITbTroubleFeedbackWriteRepository tbTroubleFeedbackWriteRepository)
        {
            _tbTroubleFeedbackWriteRepository = tbTroubleFeedbackWriteRepository;
        }

        public async Task CreateTrouble(TroubleInput input)
        {
            await _tbTroubleFeedbackWriteRepository.InsertAsync(new Tb_trouble_feedback
            {
                comments = input.comments,
                create_by = input.User_id,
                create_time = DateTime.Now,
                update_time = DateTime.Now,
                deal_status = input.deal_status,
                verify_status = input.verify_status,
                img_url = input.img_url,
                trouble1 = input.trouble1,
                trouble2 = input.trouble2,
                trouble3 = input.trouble3,
                update_by = input.User_id
            });
        }

        public BikeUploadOutput UploadTroublePic()
        {
            HttpPostedFile file = HttpContext.Current.Request.Files["filedata"];
            if (file != null)
            {
                try
                {
                    // 文件上传后的保存路径
                    string filePath = HttpContext.Current.Server.MapPath("~/Uploads/Trouble/");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fileName = Path.GetFileName(file.FileName);// 原始文件名称
                    string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                    string saveName = Guid.NewGuid() + fileExtension; // 保存文件名称

                    file.SaveAs(filePath + saveName);



                    var img = ConfigurationManager.AppSettings["ServerPath"] + "Uploads/Trouble/" + saveName;

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
        }
    }
}