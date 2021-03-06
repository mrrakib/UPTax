namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InfrastructureInstuates2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InstituteInfo", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InstituteInfo", "Code");
        }
    }
}
