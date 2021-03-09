namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Menu_permission2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuPermission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        MenuConfigId = c.Int(nullable: false),
                        IsViewPermitted = c.Boolean(nullable: false),
                        IsAddPermitted = c.Boolean(nullable: false),
                        IsEditPermitted = c.Boolean(nullable: false),
                        IsDeletePermitted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.MenuConfigs", t => t.MenuConfigId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.RoleId)
                .Index(t => t.MenuConfigId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuPermission", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.MenuPermission", "MenuConfigId", "dbo.MenuConfigs");
            DropForeignKey("dbo.MenuPermission", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.MenuPermission", "RoleId", "dbo.Roles");
            DropIndex("dbo.MenuPermission", new[] { "UpdatedBy" });
            DropIndex("dbo.MenuPermission", new[] { "CreatedBy" });
            DropIndex("dbo.MenuPermission", new[] { "MenuConfigId" });
            DropIndex("dbo.MenuPermission", new[] { "RoleId" });
            DropTable("dbo.MenuPermission");
        }
    }
}
