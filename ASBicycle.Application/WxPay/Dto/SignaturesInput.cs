using Abp.Application.Services.Dto;

namespace ASBicycle.WxPay.Dto
{
    public class SignaturesInput : IInputDto
    {
        public SignaturesInput()
        {
            type = 1;
        }

        public string out_trade_no { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public double total_fee { get; set; }

        /// <summary>
        /// 1 押金  2充值
        /// </summary>
        public int type { get; set; }
    }
}