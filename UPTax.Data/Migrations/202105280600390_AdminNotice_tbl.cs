namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminNotice_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminNotice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false),
                        UnionId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.UnionParishad", t => t.UnionId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.UnionId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdminNotice", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.AdminNotice", "UnionId", "dbo.UnionParishad");
            DropForeignKey("dbo.AdminNotice", "CreatedBy", "dbo.Users");
            DropIndex("dbo.AdminNotice", new[] { "UpdatedBy" });
            DropIndex("dbo.AdminNotice", new[] { "CreatedBy" });
            DropIndex("dbo.AdminNotice", new[] { "UnionId" });
            DropTable("dbo.AdminNotice");
        }
    }
}
