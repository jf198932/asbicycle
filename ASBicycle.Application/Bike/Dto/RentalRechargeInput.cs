using Abp.Application.Services.Dto;

namespace ASBicycle.Bike.Dto
{
    public class RentalRechargeInput : IInputDto
    {
        public string out_trade_no { get; set; }
        //public double total_fee { get; set; }
        public int? coupon_id { get; set; }
        public double? disamount { get; set; }
    }
}