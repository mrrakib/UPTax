namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_VillageInfo_Add_WardId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VillageInfo", "WardId", c => c.Int(nullable: false));
            CreateIndex("dbo.VillageInfo", "WardId");
            AddForeignKey("dbo.VillageInfo", "WardId", "dbo.WardInfo", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VillageInfo", "WardId", "dbo.WardInfo");
            DropIndex("dbo.VillageInfo", new[] { "WardId" });
            DropColumn("dbo.VillageInfo", "WardId");
        }
    }
}
