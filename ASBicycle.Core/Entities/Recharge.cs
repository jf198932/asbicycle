using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    /// <summary>
    /// 充值
    /// </summary>
    [Table("Recharge")]
    public class Recharge : Entity
    {
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        /// <summary>
        /// 预充
        /// </summary>
        public double? Recharge_count { get; set; }
        /// <summary>
        /// 失效
        /// </summary>
        public int? Recharge_sum { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public double? Deposit { get; set; }


        public int? User_id { get; set; }
        [ForeignKey("User_id")]
        public virtual User User { get; set; }
    }
}