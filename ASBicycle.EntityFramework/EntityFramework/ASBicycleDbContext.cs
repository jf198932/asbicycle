﻿using System;
using System.Data.Entity;
using Abp.EntityFramework;

namespace ASBicycle.EntityFramework
{
    public class ASBicycleDbContext : AbpDbContext
    {
        //TODO: Define an IDbSet for each Entity...
        public virtual IDbSet<Entities.User> Users { get; set; }
        public virtual IDbSet<Entities.Track> Tracks { get; set; }
        public virtual IDbSet<Entities.Sitebeacon> Sitebeacons { get; set; }
        public virtual IDbSet<Entities.School> Schools { get; set; }
        public virtual IDbSet<Entities.Refound> Refounds { get; set; }
        public virtual IDbSet<Entities.Recharge_detail> Recharge_details { get; set; }
        public virtual IDbSet<Entities.Recharge> Recharges { get; set; }
        public virtual IDbSet<Entities.Message> Messages { get; set; }
        public virtual IDbSet<Entities.Log> Logs { get; set; }
        public virtual IDbSet<Entities.Credit> Credits { get; set; }
        public virtual IDbSet<Entities.Bikesite> Bikesites { get; set; }
        public virtual IDbSet<Entities.Bike> Bikes { get; set; }
        public virtual IDbSet<Entities.Sitemonitor> Sitemonitors { get; set; }
        public virtual IDbSet<Entities.VersionUpdate> VersionUpdates { get; set; }
        public virtual IDbSet<Entities.Parameter> Parameters { get; set; } 
        public virtual IDbSet<Entities.Tb_trouble_feedback> TbTroubleFeedbacks { get; set; }
        public virtual IDbSet<Entities.Coupon> Coupons { get; set; }
        public virtual IDbSet<Entities.CouponPackage> CouponPackages { get; set; }
        public virtual IDbSet<Entities.CouponPkgAss> CouponPkgAsses { get; set; }
        public virtual IDbSet<Entities.CouponUserAss> CouponUserAsses { get; set; } 

        public virtual IDbSet<Entities.UserDevice> UserDevices { get; set; } 
        //权限
        //public virtual IDbSet<Entities.Authen.BackUser> BackUsers { get; set; }
        //public virtual IDbSet<Entities.Authen.Role> Roles { get; set; }
        //public virtual IDbSet<Entities.Authen.Module> Modules { get; set; }
        //public virtual IDbSet<Entities.Authen.Permission> Permissions { get; set; }
        //public virtual IDbSet<Entities.Authen.UserRole> UserRoles { get; set; }
        //public virtual IDbSet<Entities.Authen.ModulePermission> ModulePermissions { get; set; }
        //public virtual IDbSet<Entities.Authen.RoleModulePermission> RoleModulePermissions { get; set; }

        //Example:
        //public virtual IDbSet<User> Users { get; set; }

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public ASBicycleDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in ASBicycleDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of ASBicycleDbContext since ABP automatically handles it.
         */
        public ASBicycleDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }
    }

    public class ReadonlyASBicycleDbContext : ASBicycleDbContext
    {
        public ReadonlyASBicycleDbContext() : base("Read")
        {

        }
        public override int SaveChanges()
        {
            // Throw if they try to call this
            throw new InvalidOperationException("This context is read-only.");
        }
    }
}
