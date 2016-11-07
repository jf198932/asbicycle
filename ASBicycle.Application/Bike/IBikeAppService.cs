using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.Bike.Dto;

namespace ASBicycle.Bike
{
    public interface IBikeAppService : IApplicationService
    {
        [HttpGet]
        Task<BikeOutput> GetBikeInfo([FromUri] BikegetInput input);
        [HttpPost]
        Task UpdateBike(BikeInput bikeInput);
        [HttpPost]
        BikeUploadOutput UploadBikePic();

        [HttpGet]
        Task<AlarmBikeOutput> GetAlarmBikeWay([FromUri] BikegetInput input);
        [HttpPost]
        Task<RentalBikeOutput> RentalBike(RentalBikeInput input);
        [HttpPost]
        Task<RentalFinishOutput> RentalBikeFinish(RentalBikeInput input);
        [HttpPost]
        Task<RentalBikeOutput> RefreshBike(RentalBikeInput input);

        [HttpPost]
        Task<RentalCostOutput> RentalCast(RentalBikeInput input);


        [HttpPost]
        Task<RentalBikeOutput> RentalBiketemp(RentalBikeInput input);
        [HttpPost]
        Task<RentalCostOutput> RentalBikeFinishtemp(RentalBikeInput input);

        [HttpPost]
        Task<RentalCostOutput> RentalBikeForcedFinishtemp(RentalBikeInput input);


        [HttpPost]
        Task<RentalFinishOutput> RentalBikeFinishtempo(RentalBikeInput input);
        [HttpPost]
        Task<RentalBikeOutput> RefreshBiketemp(RentalBikeInput input);

        [HttpGet]
        Task<RentalInfoOutput> RentalFinishInfo([FromUri] RentalBikeInput input);

        [HttpGet]
        Task<TrackInfoOutput> RentalTrackInfo([FromUri] RentalBikeInput input);

        [HttpGet]
        Task<CanRentalOutput> CanReantalBike([FromUri] RentalBikeInput input);
    }
}