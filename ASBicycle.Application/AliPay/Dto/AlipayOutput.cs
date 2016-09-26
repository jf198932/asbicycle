using Abp.Application.Services.Dto;

namespace ASBicycle.AliPay.Dto
{
    public class AlipayOutput : IOutputDto
    {
         public string data { get; set; }
    }
}