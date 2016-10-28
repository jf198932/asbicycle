using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.AliPay.Dto;

namespace ASBicycle.AliPay
{
    public interface IAlipayAppService : IApplicationService
    {
        [HttpPost]
        AlipayOutput signatures(SignaturesInput input);
    }
}