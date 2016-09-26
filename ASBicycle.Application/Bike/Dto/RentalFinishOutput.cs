using Abp.Application.Services.Dto;

namespace ASBicycle.Bike.Dto
{
    public class RentalFinishOutput : IOutputDto
    {
        public string out_trade_no { get; set; }
        public string end_time { get; set; }
    }
}