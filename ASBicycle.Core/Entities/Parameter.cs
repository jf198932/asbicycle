using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    /// <summary>
    /// 公共字段表
    /// </summary>
    [Table("Parameter")]
    public class Parameter : Entity
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string program { get; set; }
        /// <summary>
        /// 字段值
        /// </summary>
        public string parameter_value { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// Y/N
        /// </summary>
        public string enable { get; set; }
        public DateTime? created_at { get; set; }

    }
}