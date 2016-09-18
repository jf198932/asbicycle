using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Web.Models;
using ASBicycle.Rental.AliPay.Dto;

namespace ASBicycle.Rental.AliPay
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