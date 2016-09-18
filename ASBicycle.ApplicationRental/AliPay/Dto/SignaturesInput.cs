using Abp.Application.Services.Dto;

namespace ASBicycle.Rental.AliPay.Dto
{
    public class SignaturesInput : IInputDto
    {
        public string out_trade_no { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string total_fee { get; set; }
//        out_trade_no
//subject
//body
//total_fee
    }
}