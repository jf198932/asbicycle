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
        Task<BikesiteOutput> GetOneBikesiteInfo([FromUri]int id, int index, int pagesize);

        [HttpGet]
        Task<List<BikesiteListOutput>> GetNearbyBikesites([FromUri]double lat, double lon);
    }
}