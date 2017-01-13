using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace ASBicycle.Bike.Dto
{
    public class RentalBikeInput : IInputDto
    {
        public int user_id { get; set; }

        public string Ble_name { get; set; }

        public string out_trade_no { get; set; }

        public string gps_point { get; set; }
        
        public string isrstr { get; set; }

        public int? coupon_id { get; set; }
        public double? disamount { get; set; }
    }
}