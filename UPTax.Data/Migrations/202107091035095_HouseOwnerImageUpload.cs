namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HouseOwnerImageUpload : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HouseOwners", "ImagePath", c => c.String());
            AddColumn("dbo.HouseOwners", "ImageName", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HouseOwners", "ImageName");
            DropColumn("dbo.HouseOwners", "ImagePath");
        }
    }
}
