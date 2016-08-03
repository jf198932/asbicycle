using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities.Authen
{
    /// <summary>
    /// 角色-模块-权限
    /// </summary>
    [Table("RoleModulePermission")]
    public class RoleModulePermission : Entity
    {
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public int? PermissionId { get; set; }
        public int? CreateId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyId { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        [ForeignKey("ModuleId")]
        public virtual Module Module { get; set; }
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}