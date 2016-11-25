using Abp.Application.Services.Dto;

namespace ASBicycle.Recharge.Dto
{
    public class RefoundOutput : IOutputDto
    {
        public int code { get; set; }
        public string msg { get; set; }
    }
}