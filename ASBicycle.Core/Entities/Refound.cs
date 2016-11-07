using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    /// <summary>
    /// 退款
    /// </summary>
    [Table("Refound")]
    public class Refound : Entity
    {
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        /// <summary>
        /// 申请退款金额
        /// </summary>
        public double? Refound_amount { get; set; }
        /// <summary>
        /// 1.申请中, 2.审核不通过，3.退款中，4.退款成功
        /// </summary>
        public int? Refound_status { get; set; }

        public int? User_id { get; set; }
        [ForeignKey("User_id")]
        public virtual User User { get; set; }
    }
}