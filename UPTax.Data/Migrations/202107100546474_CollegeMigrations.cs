namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CollegeMigrations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InstituteInfo", "YearlyRentAmount", c => c.Double());
            AddColumn("dbo.InstituteInfo", "YearlyInterestRate", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InstituteInfo", "YearlyInterestRate");
            DropColumn("dbo.InstituteInfo", "YearlyRentAmount");
        }
    }
}
