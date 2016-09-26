using Abp.Application.Services.Dto;

namespace ASBicycle.Bike.Dto
{
    public class RentalBikeOutput : IOutputDto
    {
        public string out_trade_no { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string start_site_name { get; set; }
        public string ble_name { get; set; }
        public string start_time { get; set; }
        public string end_site_name { get; set; }

        public double lat_end { get; set; }
        public double lon_end { get; set; }
    }
}