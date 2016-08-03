using Abp.Application.Services;
using ASBicycle.VersionUpdate.Dto;
using System.Threading.Tasks;
using System.Web.Http;

namespace ASBicycle.VersionUpdate
{
    public interface IVersionUpdateAppService : IApplicationService
    {
        [HttpGet]
        Task<VersionUpdateOutput> UpdateApp();
    }
}
