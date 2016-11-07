using Abp.Application.Services.Dto;

namespace ASBicycle.WxPay.Dto
{
    public class WxpayOutput : IOutputDto
    {
        public string appid { get; set; }
        public string noncestr { get; set; }
        public string package { get; set; }
        public string partnerid { get; set; }
        public string prepayid { get; set; }
        public string timestamp { get; set; }
        public string sign { get; set; }
    }
}