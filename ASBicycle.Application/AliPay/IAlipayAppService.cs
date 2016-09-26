using System.Web.Http;
using Abp.Application.Services;
using Abp.Web.Models;
using ASBicycle.AliPay.Dto;

namespace ASBicycle.AliPay
{
    public interface IAlipayAppService : IApplicationService
    {
        [HttpPost]
        AlipayOutput signatures(SignaturesInput input);
        [HttpPost, DontWrapResult]
        void return_url();
        [HttpPost, DontWrapResult]
        void notify_url();
    }
}