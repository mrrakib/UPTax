namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WardInfosForeginKey : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.WardInfo", "UnionId");
            AddForeignKey("dbo.WardInfo", "UnionId", "dbo.UnionParishad", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WardInfo", "UnionId", "dbo.UnionParishad");
            DropIndex("dbo.WardInfo", new[] { "UnionId" });
        }
    }
}
