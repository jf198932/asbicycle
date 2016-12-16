using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ASBicycle.Bikesite.Dto;

namespace ASBicycle.Bike.Dto
{
    public class RentalBikeOutput : IOutputDto
    {
        public RentalBikeOutput()
        {
            BikesiteList = new List<BikesiteEntity>();
        }

        public string out_trade_no { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string start_site_name { get; set; }
        public string ble_name { get; set; }
        public string ble_serial { get; set; }
        public int ble_type { get; set; }
        public string start_time { get; set; }
        public string end_site_name { get; set; }

        public double lat_end { get; set; }
        public double lon_end { get; set; }
        public string pwd { get; set; }

        public List<BikesiteEntity> BikesiteList { get; set; }
    }

    [AutoMapFrom(typeof(Entities.Bikesite))]
    public class BikesiteEntity : EntityDto
    {
        public string Name { get; set; }
        public int? Type { get; set; }
        public string Description { get; set; }
        public int? Rent_charge { get; set; }
        public int? Return_charge { get; set; }
        public string Gps_point { get; set; }
        public int? Radius { get; set; }
        public int? Bike_count { get; set; }
        public int? Available_count { get; set; }
        public int? School_id { get; set; }

        public double Distance { get; set; }
    }
}