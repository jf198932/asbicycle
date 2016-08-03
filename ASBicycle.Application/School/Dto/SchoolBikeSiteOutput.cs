using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ASBicycle.School.Dto
{
    public class SchoolBikeSiteOutput : IOutputDto
    {
        public int Id { get; set; }
        public int School_id { get; set; }
        public int? Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Rent_charge { get; set; }
        public int? Return_charge { get; set; }
        public string Gps_point { get; set; }
        public int? Bike_count { get; set; }
        public int? Available_count { get; set; }
        public int? Radius { get; set; }
        public int Available_bikes { get; set; }
        public List<SitebeaconDto> Beacons { get; set; }
        
    }
    [AutoMapFrom(typeof(Entities.Sitebeacon))]
    public class SitebeaconDto : EntityDto
    {
        public string Name { get; set; }
        public int Bikesite_id { get; set; }
        public string Gps_point { get; set; }
        public int Tx_power { get; set; }
        public int Battery { get; set; }
    }
}