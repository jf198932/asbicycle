using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;

namespace ASBicycle.Log.Dto
{
    public class LogInput : IInputDto
    {
        //public int Id { get; set; }
        //public DateTime? Created_at { get; set; }
        //public DateTime? Updated_at { get; set; }
        /// <summary>
        /// 1.锁车  2. 开车3.异常离开，4.用户报警，5 报警车辆
        /// </summary>
        public int Type { get; set; }
        public DateTime? Op_Time { get; set; }


        //public int? Bikesite_id { get; set; }
        //public int? Bike_id { get; set; }

        public string ble_name { get; set; }
    }
}