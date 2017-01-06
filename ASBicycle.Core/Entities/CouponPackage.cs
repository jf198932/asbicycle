using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    [Table("tb_coupon_package")]
    public class CouponPackage : Entity
    {
        public DateTime create_time { get; set; }
        public DateTime? update_time { get; set; }
        /// <summary>
        /// Key,礼包编号(唯一)，例如 ISR0000001，一个礼包券一个用户只能领取一次,可以被多个用户领取
        /// </summary>
        public string coupon_pkg_num { get; set; }
        /// <summary>
        /// 礼包名称
        /// </summary>
        public string coupon_pkg_name { get; set; }
        /// <summary>
        /// 兑换码, 系统生成一个非重复的随机序列
        /// </summary>
        public string redeem_code { get; set; }
        /// <summary>
        /// 领用上限次数
        /// </summary>
        public int upper_limit { get; set; }
        /// <summary>
        /// 礼包失效日期
        /// </summary>
        public DateTime? expire_date { get; set; }
        /// <summary>
        /// 0.可用, 1.不可用
        /// </summary>
        public int coupon_status { get; set; }
    }
}