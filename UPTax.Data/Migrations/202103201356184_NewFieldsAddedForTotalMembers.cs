namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewFieldsAddedForTotalMembers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HouseOwners", "TotalMember", c => c.Int());
            AddColumn("dbo.HouseOwners", "TotalBoys", c => c.Int());
            AddColumn("dbo.HouseOwners", "TotalGirls", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HouseOwners", "TotalGirls");
            DropColumn("dbo.HouseOwners", "TotalBoys");
            DropColumn("dbo.HouseOwners", "TotalMember");
        }
    }
}
