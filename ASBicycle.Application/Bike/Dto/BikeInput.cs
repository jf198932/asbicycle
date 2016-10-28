using Abp.Application.Services.Dto;

namespace ASBicycle.Bike.Dto
{
    public class BikeInput : IInputDto
    {
        public string Serial { get; set; }
        public int? Vlock_status { get; set; }
        public int? Bike_status { get; set; }
        public int Battery { get; set; }
        public int? Bikesite_id { get; set; }
        public string Position { get; set; }
        public string Bike_img { get; set; }
    }

    public class BikegetInput : IInputDto
    {
        public string serial { get; set; }
        public string phone { get; set; }
    }
}