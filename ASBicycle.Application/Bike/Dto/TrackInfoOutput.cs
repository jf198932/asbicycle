using Abp.Application.Services.Dto;

namespace ASBicycle.Bike.Dto
{
    public class TrackInfoOutput : IOutputDto
    {
        public string out_trade_no { get; set; }
        public int? pay_status { get; set; }
        public int ble_type { get; set; }
    }
}