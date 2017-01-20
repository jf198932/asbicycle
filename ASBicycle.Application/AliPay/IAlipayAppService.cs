using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.AliPay.Dto;

namespace ASBicycle.AliPay
{
    public interface IAlipayAppService : IApplicationService
    {
        /// <summary>
        /// 支付宝签名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        AlipayOutput signatures(SignaturesInput input);
    }
}