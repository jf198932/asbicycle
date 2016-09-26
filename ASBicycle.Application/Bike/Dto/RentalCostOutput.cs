using Abp.Application.Services.Dto;

namespace ASBicycle.Bike.Dto
{
    public class RentalCostOutput : IOutputDto
    {
        public string out_trade_no { get; set; }
        public string start_site_name { get; set; }
        public string ble_name { get; set; }
        public string start_time { get; set; }
        public string end_site_name { get; set; }
        public string end_time { get; set; }
        public string school_name { get; set; }

        public int rental_time { get; set; }
        public string allpay { get; set; }
    }
}