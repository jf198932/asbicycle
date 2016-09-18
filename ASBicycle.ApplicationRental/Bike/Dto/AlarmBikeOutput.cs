using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace ASBicycle.Rental.Bike.Dto
{
    public class AlarmBikeOutput : IOutputDto
    {
        public AlarmBikeOutput()
        {
            alarmlist = new List<AlarmBikeDto>();
        }
        public string bikename { get; set; }
        public string bikeimg { get; set; }
        public List<AlarmBikeDto> alarmlist { get; set; } 
    }

    public class AlarmBikeDto
    {
        public string alarmtime { get; set; }
        public string sitename { get; set; }
        public string gps_point { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
    }
}