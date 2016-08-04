using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities.Authen
{
    /// <summary>
    /// 权限
    /// </summary>
    [Table("Permission")]
    public class Permission : Entity
    {
        public Permission()
        {
            this.ModulePermission = new List<ModulePermission>();
            this.RoleModulePermission = new List<RoleModulePermission>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public int OrderSort { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public int? CreateId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyId { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }

        public int? School_id { get; set; }
        [ForeignKey("School_id")]
        public virtual School School { get; set; }

        public virtual ICollection<ModulePermission> ModulePermission { get; set; }
        public virtual ICollection<RoleModulePermission> RoleModulePermission { get; set; }
    }
}