using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    [Table("Log")]
    public class Log : Entity
    {
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        /// <summary>
        /// 3.异常离开，4.用户报警，5 报警车辆
        /// </summary>
        public int? Type { get; set; }
        public DateTime? Op_Time { get; set; }


        public int? Bikesite_id { get; set; }
        public int? Bike_id { get; set; }
        [ForeignKey("Bikesite_id")]
        public virtual Bikesite Bikesite { get; set; }
        [ForeignKey("Bike_id")]
        public virtual Bike Bike { get; set; }
    }
}