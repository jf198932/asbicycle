using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ASBicycle.Track.Dto
{
    [AutoMapFrom(typeof(Entities.Track))]
    public class TrackOutput : IOutputDto
    {
        public string Start_point { get; set; }
        public string End_point { get; set; }
        public int? Start_site_id { get; set; }
        public int? End_site_id { get; set; }
        public DateTime Start_time { get; set; }
        public DateTime End_time { get; set; }
        public int? Payment { get; set; }
        public int? Pay_status { get; set; }
        
        public int? Bike_id { get; set; }
    }
}