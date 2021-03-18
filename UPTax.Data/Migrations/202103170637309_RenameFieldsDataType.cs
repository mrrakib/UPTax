namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameFieldsDataType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HouseOwners", "IsTubeWell", c => c.Boolean(nullable: false));
            DropColumn("dbo.HouseOwners", "TubeWell");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HouseOwners", "TubeWell", c => c.Boolean(nullable: false));
            DropColumn("dbo.HouseOwners", "IsTubeWell");
        }
    }
}
