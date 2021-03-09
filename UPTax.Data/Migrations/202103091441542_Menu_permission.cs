namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Menu_permission : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Icon = c.String(maxLength: 250),
                        OrderNo = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.MenuConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ControllerName = c.String(nullable: false, maxLength: 250),
                        CategoryId = c.Int(nullable: false),
                        OrderNo = c.Int(nullable: false),
                        MenuName = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.MenuCategories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CategoryId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuConfigs", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.MenuConfigs", "CategoryId", "dbo.MenuCategories");
            DropForeignKey("dbo.MenuConfigs", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.MenuCategories", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.MenuCategories", "CreatedBy", "dbo.Users");
            DropIndex("dbo.MenuConfigs", new[] { "UpdatedBy" });
            DropIndex("dbo.MenuConfigs", new[] { "CreatedBy" });
            DropIndex("dbo.MenuConfigs", new[] { "CategoryId" });
            DropIndex("dbo.MenuCategories", new[] { "UpdatedBy" });
            DropIndex("dbo.MenuCategories", new[] { "CreatedBy" });
            DropTable("dbo.MenuConfigs");
            DropTable("dbo.MenuCategories");
        }
    }
}
