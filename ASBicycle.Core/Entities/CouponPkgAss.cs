using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    [Table("tb_coupon_pkg_assignment")]
    public class CouponPkgAss : Entity
    {
        public DateTime create_time { get; set; }
        public DateTime? update_time { get; set; }
        /// <summary>
        /// key,礼券包id，一个礼包券一个用户只能领取一次,可以被多个用户领取
        /// </summary>
        public int? coupon_pkg_id { get; set; }
        /// <summary>
        /// key,优惠券id
        /// </summary>
        public int? coupon_id { get; set; }
        /// <summary>
        /// 使用期起,起用时间
        /// </summary>
        public DateTime? coupon_pkg_enable_time { get; set; }
        /// <summary>
        /// 使用期止,失效时间
        /// </summary>
        public DateTime? coupon_pkg_disable_time { get; set; }

        [ForeignKey("coupon_id")]
        public virtual Coupon Coupon { get; set; }
        [ForeignKey("coupon_pkg_id")]
        public virtual CouponPackage CouponPackage { get; set; }
    }
}