using Abp.Application.Services.Dto;

namespace ASBicycle.WxPay.Dto
{
    public class SignaturesInput : IInputDto
    {
        public string out_trade_no { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public double total_fee { get; set; }
    }
}