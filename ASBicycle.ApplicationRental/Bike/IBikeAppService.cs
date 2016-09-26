using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.Rental.Bike.Dto;

namespace ASBicycle.Rental.Bike
{
    public interface IBikeAppService : IApplicationService
    {
        [HttpPost]
        Task<RentalBikeOutput> RentalBike(RentalBikeInput input);

        [HttpPost]
        Task RentalBikeFinish(RentalBikeInput input);
    }
}