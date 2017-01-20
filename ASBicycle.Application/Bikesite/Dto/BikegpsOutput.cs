using Abp.Application.Services.Dto;

namespace ASBicycle.Bikesite.Dto
{
    public class BikegpsOutput : IOutputDto
    {
        public string ble_name { get; set; }
        public string gps { get; set; }
        public double distance { get; set; }
    }
}