using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    /// <summary>
    /// 小桩
    /// </summary>
    [Table("Sitemonitor")]
    public class Sitemonitor : Entity
    {
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        /// <summary>
        /// 监控桩位编号
        /// </summary>
        [MaxLength(32)]
        public string Name { get; set; }
        /// <summary>
        /// 停车港
        /// </summary>
        public int? Bikesite_id { get; set; }
        /// <summary>
        /// 0 正常 ,1 非工作
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; }

        [ForeignKey("Bikesite_id")]
        public virtual Bikesite Bikesite { get; set; }
    }
}