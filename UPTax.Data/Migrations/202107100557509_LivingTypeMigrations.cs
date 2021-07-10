namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LivingTypeMigrations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InstituteInfo", "LivingType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InstituteInfo", "LivingType");
        }
    }
}
