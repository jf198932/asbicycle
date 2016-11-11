using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.Recharge.Dto;

namespace ASBicycle.Recharge
{
    public interface IRechargeAppService : IApplicationService
    {
        [HttpPost]
        Task<RechargeOutput> CreateRecharge(RechargeInput input);

        [HttpPost]
        Task ApplyRefound(RefoundInput input);
    }
}