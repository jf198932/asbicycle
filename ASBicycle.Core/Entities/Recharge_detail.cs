using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    /// <summary>
    /// 充值退款记录
    /// </summary>
    [Table("Recharge_detail")]
    public class Recharge_detail : Entity
    {
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }

        public int? User_id { get; set; }

        public double? Recharge_amount { get; set; }
        /// <summary>
        /// 1.充值，2.退款
        /// </summary>
        public int? Type { get; set; }
        /// <summary>
        /// 1.押金, 2,预充值
        /// </summary>
        public int? Recharge_type { get; set; }
        /// <summary>
        /// 1.支付宝 2.微信 3.银联
        /// </summary>
        public int? Recharge_method { get; set; }
        /// <summary>
        /// 充值内部编号
        /// </summary>
        public string recharge_docno { get; set; }
        /// <summary>
        /// 支付宝/微信/银联充值单号
        /// </summary>
        public string doc_no { get; set; }

        [ForeignKey("User_id")]
        public virtual User User { get; set; }
    }
}