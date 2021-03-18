namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TOtalBuildingHouse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HouseOwners", "InfrastructureTypeId", "dbo.InfrastructureInfo");
            DropIndex("dbo.HouseOwners", new[] { "InfrastructureTypeId" });
            AddColumn("dbo.HouseOwners", "TotalBuildingHouse", c => c.Int(nullable: false));
            AddColumn("dbo.HouseOwners", "TotalSemiBuildingHouse", c => c.Int(nullable: false));
            AddColumn("dbo.HouseOwners", "TotalRawHouse", c => c.Int(nullable: false));
            DropColumn("dbo.HouseOwners", "InfrastructureTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HouseOwners", "InfrastructureTypeId", c => c.Int(nullable: false));
            DropColumn("dbo.HouseOwners", "TotalRawHouse");
            DropColumn("dbo.HouseOwners", "TotalSemiBuildingHouse");
            DropColumn("dbo.HouseOwners", "TotalBuildingHouse");
            CreateIndex("dbo.HouseOwners", "InfrastructureTypeId");
            AddForeignKey("dbo.HouseOwners", "InfrastructureTypeId", "dbo.InfrastructureInfo", "Id", cascadeDelete: true);
        }
    }
}
