using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.Log.Dto;

namespace ASBicycle.Log
{
    public interface ILogAppService : IApplicationService
    {
        [HttpPost]
        Task CreateLog(LogInput input);
    }
}