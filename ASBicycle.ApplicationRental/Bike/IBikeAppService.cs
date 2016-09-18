using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.Rental.Bike.Dto;

namespace ASBicycle.Rental.Bike
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

        [HttpPost]
        Task<RentalBikeOutput> RentalBike(RentalBikeInput input);

        [HttpPost]
        Task RentalBikeFinish(RentalBikeInput input);
    }
}