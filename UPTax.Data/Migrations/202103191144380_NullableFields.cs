namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HouseOwners", "TotalBuildingHouse", c => c.Int());
            AlterColumn("dbo.HouseOwners", "TotalSemiBuildingHouse", c => c.Int());
            AlterColumn("dbo.HouseOwners", "TotalRawHouse", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HouseOwners", "TotalRawHouse", c => c.Int(nullable: false));
            AlterColumn("dbo.HouseOwners", "TotalSemiBuildingHouse", c => c.Int(nullable: false));
            AlterColumn("dbo.HouseOwners", "TotalBuildingHouse", c => c.Int(nullable: false));
        }
    }
}
