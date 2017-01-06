using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    [Table("tb_coupon_user_assignment")]
    public class CouponUserAss : Entity
    {
        public DateTime create_time { get; set; }
        public DateTime? update_time { get; set; }
        /// <summary>
        /// key,用户id
        /// </summary>
        public int? user_id { get; set; }
        /// <summary>
        /// key,礼包券id，一个礼包券一个用户只能领取一次,可以被多个用户领取
        /// </summary>
        public int? coupon_pkg_id { get; set; }
        /// <summary>
        /// key,优惠券id
        /// </summary>
        public int? coupon_id { get; set; }
        /// <summary>
        /// 领用时间
        /// </summary>
        public DateTime? lead_time { get; set; }
        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? coupon_use_time { get; set; }
        /// <summary>
        /// 邀请码
        /// </summary>
        public string invite_code { get; set; }
        /// <summary>
        /// 邀请人
        /// </summary>
        public int? invite_by { get; set; }

        [ForeignKey("user_id")]
        public virtual User User { get; set; }
        [ForeignKey("coupon_id")]
        public virtual Coupon Coupon { get; set; }
        [ForeignKey("coupon_pkg_id")]
        public virtual CouponPackage CouponPackage { get; set; }
    }
}