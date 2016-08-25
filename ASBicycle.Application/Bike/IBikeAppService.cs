using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.Bike.Dto;

namespace ASBicycle.Bike
{
    public interface IBikeAppService : IApplicationService
    {
        [HttpGet]
        Task<BikeOutput> GetBikeInfo([FromUri]string serial);
        [HttpPost]
        Task UpdateBike(BikeInput bikeInput);
        [HttpPost]
        Task<BikeUploadOutput> UploadBikePic();

        [HttpGet]
        Task<AlarmBikeOutput> GetAlarmBikeWay([FromUri] string phone);
    }
}