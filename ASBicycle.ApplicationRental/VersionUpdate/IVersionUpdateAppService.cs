using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.Rental.VersionUpdate.Dto;

namespace ASBicycle.Rental.VersionUpdate
{
    public interface IVersionUpdateAppService : IApplicationService
    {
        [HttpGet]
        Task<VersionUpdateOutput> UpdateApp([FromUri] VersionUpdateInput input);
    }
}
