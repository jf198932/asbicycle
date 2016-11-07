using Abp.Application.Services.Dto;

namespace ASBicycle.Recharge.Dto
{
    public class RechargeOutput : IOutputDto
    {
         public string out_trade_no { get; set; }
    }
}