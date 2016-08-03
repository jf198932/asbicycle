using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;

namespace ASBicycle.XInGe
{
    public interface IXinGeAppService : IApplicationService
    {
        [HttpGet]
        void SendAndroid(string account);
        [HttpGet]
        void SendIos(string account);
    }
}