using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ASBicycle.Rental.Bike.Dto
{
    [AutoMapFrom(typeof(Entities.Bike))]
    public class BikeDto : EntityDto
    {
        public string Ble_serial { get; set; }
        public string Ble_name { get; set; }
        public int? Ble_type { get; set; }
        public int? Lock_status { get; set; }
        public int? Bike_status { get; set; }
        public int? Vlock_status { get; set; }
        public string Position { get; set; }
        public int? Battery { get; set; }
        public int? User_id { get; set; }
        public int? Bikesite_id { get; set; }
        public int? Insite_status { get; set; }
        public string Bike_img { get; set; }

        public string Bikesite_name { get; set; }
        //public int School_id { get; set; }
        //public string School_name { get; set; }
    }
}