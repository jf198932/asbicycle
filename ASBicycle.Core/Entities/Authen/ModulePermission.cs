using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities.Authen
{
    /// <summary>
    /// 模块-权限
    /// </summary>
    [Table("ModulePermission")]
    public class ModulePermission : Entity
    {
        public int ModuleId { get; set; }
        public int PermissionId { get; set; }
        public int? CreateId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyId { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        [ForeignKey("ModuleId")]
        public virtual Module Module { get; set; }
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}