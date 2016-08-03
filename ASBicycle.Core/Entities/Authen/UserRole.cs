using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities.Authen
{
    /// <summary>
    /// 用户-角色
    /// </summary>
    [Table("UserRole")]
    public class UserRole : Entity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public int? CreateId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyId { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }

        [ForeignKey("UserId")]
        public virtual BackUser BackUser { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}