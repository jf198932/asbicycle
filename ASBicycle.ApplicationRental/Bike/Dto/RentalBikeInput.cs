using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace ASBicycle.Rental.Bike.Dto
{
    public class RentalBikeInput : IInputDto
    {
        public int user_id { get; set; }

        public string Ble_name { get; set; }
        [Required]
        public int bikesiteid { get; set; }
        [Required]
        public string gps_point { get; set; }

        public string out_trade_no { get; set; }
    }
}