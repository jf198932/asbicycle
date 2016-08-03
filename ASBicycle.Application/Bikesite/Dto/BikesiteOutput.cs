using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ASBicycle.Bike.Dto;

namespace ASBicycle.Bikesite.Dto
{
    //[AutoMapFrom(typeof(Entities.Bikesite))]
    public class BikesiteOutput : IOutputDto
    {
        //public string Name { get; set; }
        //public int? Type { get; set; }
        //public string Description { get; set; }
        //public int? Rent_charge { get; set; }
        //public int? Return_charge { get; set; }
        //public string Gps_point { get; set; }
        //public int? Radius { get; set; }
        //public int? Bike_count { get; set; }
        //public int? Available_count { get; set; }
        //public int? School_id { get; set; }

        public List<BikeDto> Bikes { get; set; }
    }
    [AutoMapFrom(typeof(Entities.Bikesite))]
    public class BikesiteListOutput : IOutputDto
    {
        public int Id { get; set; }
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