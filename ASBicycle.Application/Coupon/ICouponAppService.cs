using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.Coupon.Dto;

namespace ASBicycle.Coupon
{
    public interface ICouponAppService : IApplicationService
    {
        /// <summary>
        /// 获取用户的优惠券
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        Task<CouponListOutput> GetAllUserCouponList([FromUri] CouPonInput input);

        /// <summary>
        /// 获取用户可以使用的优惠券
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        Task<CouponOutput> GetUseUserCouponList([FromUri] CouPonInput input);

        /// <summary>
        /// 兑换码换优惠券
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task GetCouponByCode(CouPonInput input);
    }
}