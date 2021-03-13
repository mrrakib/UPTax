namespace UPTax.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialHouseOwnerWardInfoId : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.HouseOwners", "WardInfoId");
            AddForeignKey("dbo.HouseOwners", "WardInfoId", "dbo.WardInfo", "Id", cascadeDelete: false);
        }

        public override void Down()
        {
            DropForeignKey("dbo.HouseOwners", "WardInfoId", "dbo.WardInfo");
            DropIndex("dbo.HouseOwners", new[] { "WardInfoId" });
        }
    }
}
