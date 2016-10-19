using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.Bikesite.Dto;

namespace ASBicycle.Bikesite
{
    public interface IBikesiteAppService : IApplicationService
    {
        [HttpGet]
        Task<BikesiteOutput> GetOneBikesiteInfo([FromUri]BikesitePageInput input);

        [HttpGet]
        Task<List<BikesiteListOutput>> GetNearbyBikesites([FromUri]BikesiteInput input);
    }
}