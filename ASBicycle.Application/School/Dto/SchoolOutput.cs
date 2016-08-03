using System;
using Abp.Application.Services.Dto;

namespace ASBicycle.School.Dto
{
    public class SchoolOutput : IOutputDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Areacode { get; set; }
        public string Gps_point { get; set; }
        public int? Site_count { get; set; }
        public int? Bike_count { get; set; }
        public int? Time_charge { get; set; }
        public DateTime? Refresh_date { get; set; }
    }
}