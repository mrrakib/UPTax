namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WardInfosCreatedUpdatedUserForeginKey : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WardInfo", "CreatedBy", c => c.String(maxLength: 128));
            AlterColumn("dbo.WardInfo", "UpdatedBy", c => c.String(maxLength: 128));
            CreateIndex("dbo.WardInfo", "CreatedBy");
            CreateIndex("dbo.WardInfo", "UpdatedBy");
            AddForeignKey("dbo.WardInfo", "CreatedBy", "dbo.Users", "Id");
            AddForeignKey("dbo.WardInfo", "UpdatedBy", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WardInfo", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.WardInfo", "CreatedBy", "dbo.Users");
            DropIndex("dbo.WardInfo", new[] { "UpdatedBy" });
            DropIndex("dbo.WardInfo", new[] { "CreatedBy" });
            AlterColumn("dbo.WardInfo", "UpdatedBy", c => c.Int());
            AlterColumn("dbo.WardInfo", "CreatedBy", c => c.Int(nullable: false));
        }
    }
}
