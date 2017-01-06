using System.Collections.Generic;
using Abp.Application.Services.Dto;
using ASBicycle.Bike.Dto;

namespace ASBicycle.Coupon.Dto
{
    public class CouponOutput : IOutputDto
    {
        public CouponOutput()
        {
            Canuse = new List<CouponDto>();
        }
        /// <summary>
        /// 可使用
        /// </summary>
        public List<CouponDto> Canuse { get; set; }
    }
}