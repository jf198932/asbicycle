using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.WxPay.Dto;

namespace ASBicycle.WxPay
{
    public interface IWxpayAppService : IApplicationService
    {
        [HttpPost]
        WxpayOutput signatures(SignaturesInput input);
    }
}