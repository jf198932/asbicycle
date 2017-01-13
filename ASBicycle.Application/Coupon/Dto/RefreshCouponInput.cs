using Abp.Application.Services.Dto;

namespace ASBicycle.Coupon.Dto
{
    public class RefreshCouponInput : IInputDto
    {
        public double oldpay { get; set; }
        public int type { get; set; }
        public double discount { get; set; }
    }
}