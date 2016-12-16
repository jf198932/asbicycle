using Abp.Application.Services.Dto;

namespace ASBicycle.Bike.Dto
{
    public class RentalRechargeInput : IInputDto
    {
        public string out_trade_no { get; set; }
        public string total_fee { get; set; }
    }
}