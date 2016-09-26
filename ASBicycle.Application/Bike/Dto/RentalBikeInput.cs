using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace ASBicycle.Bike.Dto
{
    public class RentalBikeInput : IInputDto
    {
        public int user_id { get; set; }

        public string Ble_name { get; set; }

        public string out_trade_no { get; set; }
    }
}