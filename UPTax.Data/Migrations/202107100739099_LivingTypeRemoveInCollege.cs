namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LivingTypeRemoveInCollege : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.InstituteInfo", "LivingType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InstituteInfo", "LivingType", c => c.String());
        }
    }
}
