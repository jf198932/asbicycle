using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ASBicycle.Bike.Dto
{
    [AutoMapFrom(typeof(Entities.Bike))]
    public class BikeOutput : IOutputDto
    {
        public string Ble_serial { get; set; }
        public string Ble_name { get; set; }
        public int Bike_status { get; set; }
        //public int School_id { get; set; }
        public int Bikesite_id { get; set; }
        public int Battery { get; set; }
        public string Bike_img { get; set; }

        public string Bikesite_name { get; set; }
    }
}