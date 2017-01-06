using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    [Table("tb_coupon")]
    public class Coupon : Entity
    {
        public DateTime create_time { get; set; }
        public DateTime? update_time { get; set; }
        /// <summary>
        /// 优惠券名称(key)
        /// </summary>
        public string coupon_name { get; set; }
        /// <summary>
        /// 优惠券使用规则
        /// </summary>
        public string coupon_rule { get; set; }
        /// <summary>
        /// 1,抵用券 2,折扣券
        /// </summary>
        public int coupon_type { get; set; }
        /// <summary>
        /// 抵用券单位元,折扣券是百分比
        /// </summary>
        public double coupon_value { get; set; }
        /// <summary>
        /// 0.可用, 1.不可用
        /// </summary>
        public int coupon_status { get; set; }
    }
}