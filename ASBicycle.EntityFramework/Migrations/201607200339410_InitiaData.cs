namespace ASBicycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitiaData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BackUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LoginName = c.String(unicode: false),
                        LoginPwd = c.String(unicode: false),
                        FullName = c.String(unicode: false),
                        Email = c.String(unicode: false),
                        Phone = c.String(unicode: false),
                        Enabled = c.Boolean(nullable: false),
                        PwdErrorCount = c.Int(nullable: false),
                        LoginCount = c.Int(nullable: false),
                        RegisterTime = c.DateTime(precision: 0),
                        LastLoginTime = c.DateTime(precision: 0),
                        CreateId = c.Int(),
                        CreateBy = c.String(unicode: false),
                        CreateTime = c.DateTime(precision: 0),
                        ModifyId = c.Int(),
                        ModifyBy = c.String(unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        CreateId = c.Int(),
                        CreateBy = c.String(unicode: false),
                        CreateTime = c.DateTime(precision: 0),
                        ModifyId = c.Int(),
                        ModifyBy = c.String(unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BackUser", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        OrderSort = c.Int(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        CreateId = c.Int(),
                        CreateBy = c.String(unicode: false),
                        CreateTime = c.DateTime(precision: 0),
                        ModifyId = c.Int(),
                        ModifyBy = c.String(unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleModulePermission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        ModuleId = c.Int(nullable: false),
                        PermissionId = c.Int(),
                        CreateId = c.Int(),
                        CreateBy = c.String(unicode: false),
                        CreateTime = c.DateTime(precision: 0),
                        ModifyId = c.Int(),
                        ModifyBy = c.String(unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Permission", t => t.PermissionId)
                .ForeignKey("dbo.Module", t => t.ModuleId, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.ModuleId)
                .Index(t => t.PermissionId);
            
            CreateTable(
                "dbo.Module",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(),
                        Name = c.String(unicode: false),
                        LinkUrl = c.String(unicode: false),
                        Area = c.String(unicode: false),
                        Controller = c.String(unicode: false),
                        Action = c.String(unicode: false),
                        Icon = c.String(unicode: false),
                        Code = c.String(unicode: false),
                        OrderSort = c.Int(nullable: false),
                        Description = c.String(unicode: false),
                        IsMenu = c.Boolean(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        CreateId = c.Int(),
                        CreateBy = c.String(unicode: false),
                        CreateTime = c.DateTime(precision: 0),
                        ModifyId = c.Int(),
                        ModifyBy = c.String(unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Module", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.ModulePermission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModuleId = c.Int(nullable: false),
                        PermissionId = c.Int(nullable: false),
                        CreateId = c.Int(),
                        CreateBy = c.String(unicode: false),
                        CreateTime = c.DateTime(precision: 0),
                        ModifyId = c.Int(),
                        ModifyBy = c.String(unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Module", t => t.ModuleId, cascadeDelete: true)
                .ForeignKey("dbo.Permission", t => t.PermissionId, cascadeDelete: true)
                .Index(t => t.ModuleId)
                .Index(t => t.PermissionId);
            
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(unicode: false),
                        Name = c.String(unicode: false),
                        OrderSort = c.Int(nullable: false),
                        Icon = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        Enabled = c.Boolean(nullable: false),
                        CreateId = c.Int(),
                        CreateBy = c.String(unicode: false),
                        CreateTime = c.DateTime(precision: 0),
                        ModifyId = c.Int(),
                        ModifyBy = c.String(unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bike",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created_at = c.DateTime(precision: 0),
                        Updated_at = c.DateTime(precision: 0),
                        Ble_serial = c.String(maxLength: 32, storeType: "nvarchar"),
                        Ble_name = c.String(maxLength: 32, storeType: "nvarchar"),
                        Ble_type = c.Int(),
                        Lock_status = c.Int(),
                        Bike_status = c.Int(),
                        Vlock_status = c.Int(),
                        Insite_status = c.Int(),
                        Position = c.String(maxLength: 32, storeType: "nvarchar"),
                        Battery = c.Int(),
                        User_id = c.Int(),
                        School_id = c.Int(),
                        Bikesite_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bikesite", t => t.Bikesite_id)
                .ForeignKey("dbo.User", t => t.User_id)
                .Index(t => t.User_id)
                .Index(t => t.Bikesite_id);
            
            CreateTable(
                "dbo.Bikesite",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created_at = c.DateTime(precision: 0),
                        Updated_at = c.DateTime(precision: 0),
                        Name = c.String(maxLength: 32, storeType: "nvarchar"),
                        Type = c.Int(),
                        Description = c.String(maxLength: 500, storeType: "nvarchar"),
                        Rent_charge = c.Int(),
                        Return_charge = c.Int(),
                        Gps_point = c.String(maxLength: 100, storeType: "nvarchar"),
                        Radius = c.Int(),
                        Bike_count = c.Int(),
                        Available_count = c.Int(),
                        School_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.School", t => t.School_id)
                .Index(t => t.School_id);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created_at = c.DateTime(precision: 0),
                        Updated_at = c.DateTime(precision: 0),
                        Message_info = c.String(maxLength: 1000, storeType: "nvarchar"),
                        Send_Time = c.DateTime(precision: 0),
                        Bikesite_id = c.Int(),
                        User_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bikesite", t => t.Bikesite_id)
                .ForeignKey("dbo.User", t => t.User_id)
                .Index(t => t.Bikesite_id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created_at = c.DateTime(precision: 0),
                        Updated_at = c.DateTime(precision: 0),
                        Phone = c.String(maxLength: 32, storeType: "nvarchar"),
                        Name = c.String(maxLength: 32, storeType: "nvarchar"),
                        Nickname = c.String(maxLength: 32, storeType: "nvarchar"),
                        Weixacc = c.String(maxLength: 32, storeType: "nvarchar"),
                        Email = c.String(maxLength: 32, storeType: "nvarchar"),
                        Certification = c.Int(),
                        Textmsg = c.Int(),
                        Textmsg_time = c.DateTime(precision: 0),
                        Remember_token = c.String(maxLength: 100, storeType: "nvarchar"),
                        Credits = c.Int(),
                        Balance = c.Int(),
                        School_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.School", t => t.School_id)
                .Index(t => t.School_id);
            
            CreateTable(
                "dbo.Credit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created_at = c.DateTime(precision: 0),
                        Updated_at = c.DateTime(precision: 0),
                        Credits_count = c.Int(),
                        Credits_sum = c.Int(),
                        User_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.Recharge",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created_at = c.DateTime(precision: 0),
                        Updated_at = c.DateTime(precision: 0),
                        Recharge_count = c.Int(),
                        Recharge_sum = c.Int(),
                        User_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.Refound",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created_at = c.DateTime(precision: 0),
                        Updated_at = c.DateTime(precision: 0),
                        Refound_count = c.Int(),
                        User_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.School",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created_at = c.DateTime(precision: 0),
                        Updated_at = c.DateTime(precision: 0),
                        Name = c.String(maxLength: 32, storeType: "nvarchar"),
                        Areacode = c.String(maxLength: 32, storeType: "nvarchar"),
                        Gps_point = c.String(maxLength: 100, storeType: "nvarchar"),
                        Site_count = c.Int(),
                        Bike_count = c.Int(),
                        Time_charge = c.Int(),
                        Refresh_date = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sitebeacon",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created_at = c.DateTime(precision: 0),
                        Updated_at = c.DateTime(precision: 0),
                        Name = c.String(maxLength: 32, storeType: "nvarchar"),
                        Gps_point = c.String(maxLength: 100, storeType: "nvarchar"),
                        Tx_power = c.Int(),
                        Battery = c.Int(),
                        Bikesite_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bikesite", t => t.Bikesite_id)
                .Index(t => t.Bikesite_id);
            
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created_at = c.DateTime(precision: 0),
                        Updated_at = c.DateTime(precision: 0),
                        Type = c.Int(),
                        Op_Time = c.DateTime(precision: 0),
                        Bikesite_id = c.Int(nullable: false),
                        Bike_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bike", t => t.Bike_id)
                .Index(t => t.Bike_id);
            
            CreateTable(
                "dbo.Track",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created_at = c.DateTime(precision: 0),
                        Updated_at = c.DateTime(precision: 0),
                        Start_point = c.String(maxLength: 100, storeType: "nvarchar"),
                        End_point = c.String(maxLength: 100, storeType: "nvarchar"),
                        Start_site_id = c.Int(),
                        End_site_id = c.Int(),
                        Start_time = c.DateTime(nullable: false, precision: 0),
                        End_time = c.DateTime(nullable: false, precision: 0),
                        Payment = c.Int(),
                        Pay_status = c.Int(),
                        User_id = c.Int(nullable: false),
                        Bike_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bike", t => t.Bike_id)
                .Index(t => t.Bike_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Track", "Bike_id", "dbo.Bike");
            DropForeignKey("dbo.Log", "Bike_id", "dbo.Bike");
            DropForeignKey("dbo.Sitebeacon", "Bikesite_id", "dbo.Bikesite");
            DropForeignKey("dbo.User", "School_id", "dbo.School");
            DropForeignKey("dbo.Bikesite", "School_id", "dbo.School");
            DropForeignKey("dbo.Refound", "User_id", "dbo.User");
            DropForeignKey("dbo.Recharge", "User_id", "dbo.User");
            DropForeignKey("dbo.Message", "User_id", "dbo.User");
            DropForeignKey("dbo.Credit", "User_id", "dbo.User");
            DropForeignKey("dbo.Bike", "User_id", "dbo.User");
            DropForeignKey("dbo.Message", "Bikesite_id", "dbo.Bikesite");
            DropForeignKey("dbo.Bike", "Bikesite_id", "dbo.Bikesite");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.RoleModulePermission", "RoleId", "dbo.Role");
            DropForeignKey("dbo.RoleModulePermission", "ModuleId", "dbo.Module");
            DropForeignKey("dbo.RoleModulePermission", "PermissionId", "dbo.Permission");
            DropForeignKey("dbo.ModulePermission", "PermissionId", "dbo.Permission");
            DropForeignKey("dbo.ModulePermission", "ModuleId", "dbo.Module");
            DropForeignKey("dbo.Module", "ParentId", "dbo.Module");
            DropForeignKey("dbo.UserRole", "UserId", "dbo.BackUser");
            DropIndex("dbo.Track", new[] { "Bike_id" });
            DropIndex("dbo.Log", new[] { "Bike_id" });
            DropIndex("dbo.Sitebeacon", new[] { "Bikesite_id" });
            DropIndex("dbo.Refound", new[] { "User_id" });
            DropIndex("dbo.Recharge", new[] { "User_id" });
            DropIndex("dbo.Credit", new[] { "User_id" });
            DropIndex("dbo.User", new[] { "School_id" });
            DropIndex("dbo.Message", new[] { "User_id" });
            DropIndex("dbo.Message", new[] { "Bikesite_id" });
            DropIndex("dbo.Bikesite", new[] { "School_id" });
            DropIndex("dbo.Bike", new[] { "Bikesite_id" });
            DropIndex("dbo.Bike", new[] { "User_id" });
            DropIndex("dbo.ModulePermission", new[] { "PermissionId" });
            DropIndex("dbo.ModulePermission", new[] { "ModuleId" });
            DropIndex("dbo.Module", new[] { "ParentId" });
            DropIndex("dbo.RoleModulePermission", new[] { "PermissionId" });
            DropIndex("dbo.RoleModulePermission", new[] { "ModuleId" });
            DropIndex("dbo.RoleModulePermission", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropTable("dbo.Track");
            DropTable("dbo.Log");
            DropTable("dbo.Sitebeacon");
            DropTable("dbo.School");
            DropTable("dbo.Refound");
            DropTable("dbo.Recharge");
            DropTable("dbo.Credit");
            DropTable("dbo.User");
            DropTable("dbo.Message");
            DropTable("dbo.Bikesite");
            DropTable("dbo.Bike");
            DropTable("dbo.Permission");
            DropTable("dbo.ModulePermission");
            DropTable("dbo.Module");
            DropTable("dbo.RoleModulePermission");
            DropTable("dbo.Role");
            DropTable("dbo.UserRole");
            DropTable("dbo.BackUser");
        }
    }
}
