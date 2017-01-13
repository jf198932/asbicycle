using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    /// <summary>
    /// 行程单
    /// </summary>
    [Table("Track")]
    public class Track : Entity
    {
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        /// <summary>
        /// 租车人起始GPS
        /// </summary>
        [MaxLength(100)]
        public string Start_point { get; set; }
        /// <summary>
        /// 租车人结束GPS
        /// </summary>
        [MaxLength(100)]
        public string End_point { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? Start_time { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? End_time { get; set; }
        /// <summary>
        /// 实际支付金额
        /// </summary>
        public double? Payment { get; set; }
        /// <summary>
        /// 应该支付金额
        /// </summary>
        public double? Should_pay { get; set; }
        /// <summary>
        /// 结算状态，1,待还车, 2. 还车待支付, 3. 支付完成
        /// </summary>
        public int? Pay_status { get; set; }
        /// <summary>
        /// 1.支付宝 2.微信 3.银联
        /// </summary>
        public string Pay_method { get; set; }
        /// <summary>
        /// 订单内部编号
        /// </summary>
        public string Pay_docno { get; set; }
        /// <summary>
        /// 用户建议
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 支付宝/微信/银联产生的单号
        /// </summary>
        public string Trade_no { get; set; }
        /// <summary>
        /// 所属用户
        /// </summary>
        public int User_id { get; set; }
        /// <summary>
        /// 所属用户当前租用的自行车
        /// </summary>
        public int? Bike_id { get; set; }
        /// <summary>
        /// 起始桩点GPS
        /// </summary>
        public int? Start_site_id { get; set; }
        /// <summary>
        /// 结束桩点GPS
        /// </summary>
        public int? End_site_id { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? Pay_time { get; set; }
        /// <summary>
        /// 优惠券ID
        /// </summary>
        public int? coupon_id { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public double? discount_amount { get; set; }

        //[ForeignKey("User_id")]
        //public virtual User User { get; set; }
        [ForeignKey("Bike_id")]
        public virtual Bike Bike { get; set; }
        [ForeignKey("Start_site_id")]
        public virtual Bikesite Bikesitestart { get; set; }
        [ForeignKey("End_site_id")]
        public virtual Bikesite Bikesiteend { get; set; }
    }
}