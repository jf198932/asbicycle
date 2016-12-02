using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using ASBicycle.Entities.Authen;

namespace ASBicycle.Entities
{
    /// <summary>
    /// 学校
    /// </summary>
    [Table("School")]
    public class School : Entity
    {
        public School()
        {
            //Bikes = new List<Bike>();
            Bikesites = new List<Bikesite>();
            Users = new List<User>();
            BackUsers = new List<BackUser>();
            Modules = new List<Module>();
            Permissions = new List<Permission>();
            Roles = new List<Role>();
        }

        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        [MaxLength(32)]
        public string TenancyName { get; set; }
        /// <summary>
        /// 校区名称
        /// </summary>
        [MaxLength(32)]
        public string Name { get; set; }
        /// <summary>
        /// 校区所在城市的区号
        /// </summary>
        [MaxLength(32)]
        public string Areacode { get; set; }
        /// <summary>
        /// 校区在地图上显示位置的GPS点，经度纬度逗号分隔，例如“7876566788,2342342342”	
        /// </summary>
        [MaxLength(100)]
        public string Gps_point { get; set; }
        /// <summary>
        /// 所属停车港的数量
        /// </summary>
        public int? Site_count { get; set; }
        /// <summary>
        /// 所属公共自行车的数量，防盗自行车数量不统计
        /// </summary>
        public int? Bike_count { get; set; }
        /// <summary>
        /// 公共自行车单价，单位为“RMB分/minute"，每分钟几分钱
        /// </summary>
        public int? Time_charge { get; set; }
        /// <summary>
        /// 如果所属的停车港有变化，则重置此数据为当前时间
        /// </summary>
        public DateTime? Refresh_date { get; set; }
        /// <summary>
        /// 免费时间（分钟）
        /// </summary>
        public int? Free_time { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public double? Deposit { get; set; }
        /// <summary>
        /// 固定金额
        /// </summary>
        public double? Fixed_amount { get; set; }

        //public virtual ICollection<Bike> Bikes { get; set; } 
        public virtual ICollection<Bikesite> Bikesites { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<BackUser> BackUsers { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}