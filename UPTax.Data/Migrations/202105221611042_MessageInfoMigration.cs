namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageInfoMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessageInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        ToAdminUserId = c.String(),
                        ToSupperAdminUserId = c.String(),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MessageInfo", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.MessageInfo", "CreatedBy", "dbo.Users");
            DropIndex("dbo.MessageInfo", new[] { "UpdatedBy" });
            DropIndex("dbo.MessageInfo", new[] { "CreatedBy" });
            DropTable("dbo.MessageInfo");
        }
    }
}
