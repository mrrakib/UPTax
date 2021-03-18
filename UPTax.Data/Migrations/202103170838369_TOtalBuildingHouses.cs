namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TOtalBuildingHouses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HouseOwners", "YearlyInterestRate", c => c.Double());
            DropColumn("dbo.HouseOwners", "YearlyLoanAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HouseOwners", "YearlyLoanAmount", c => c.Double());
            DropColumn("dbo.HouseOwners", "YearlyInterestRate");
        }
    }
}
