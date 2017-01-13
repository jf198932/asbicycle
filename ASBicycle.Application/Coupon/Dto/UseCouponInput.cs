using Abp.Application.Services.Dto;

namespace ASBicycle.Coupon.Dto
{
    public class UseCouponInput : IInputDto
    {
        public string out_trade_no { get; set; }
        public int coupon_id { get; set; }
        public double disamount { get; set; }
    }
}