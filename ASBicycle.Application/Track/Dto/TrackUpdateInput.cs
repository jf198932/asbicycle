using Abp.Application.Services.Dto;

namespace ASBicycle.Track.Dto
{
    public class TrackInsertInput : IInputDto
    {
        public int User_id { get; set; }
        public int Bike_id { get; set; }
        public int Start_stie_id { get; set; }
        public string Start_point { get; set; }
        public string Token { get; set; }
    }
    public class TrackUpdateInput : IInputDto
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Battery { get; set; }
        public int End_stie_id { get; set; }
        public string End_point { get; set; }
        public string Token { get; set; }
    }
}