using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.Bike.Dto;
using ASBicycle.TbTroubleFeedback.Dto;

namespace ASBicycle.TbTroubleFeedback
{
    public interface ITroubleAppService : IApplicationService
    {
        [HttpPost]
        Task CreateTrouble(TroubleInput input);

        [HttpPost]
        BikeUploadOutput UploadTroublePic();
    }
}