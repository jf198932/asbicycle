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
        public string Start_site_name { get; set; }
        public int? End_site_id { get; set; }
        public string End_site_name { get; set; }
        public string Start_time { get; set; }
        public string End_time { get; set; }
        public double? Payment { get; set; }
        public double? Should_pay { get; set; }
        public int? Pay_status { get; set; }
        public string Remark { get; set; }
        public int? Remarkstatus { get; set; }

        public string out_trade_no { get; set; }


        public int? Bike_id { get; set; }
    }
}