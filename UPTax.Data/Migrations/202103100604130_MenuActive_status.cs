namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MenuActive_status : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuConfigs", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuConfigs", "IsActive");
        }
    }
}
