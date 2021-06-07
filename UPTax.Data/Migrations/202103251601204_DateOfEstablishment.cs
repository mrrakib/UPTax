namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateOfEstablishment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InstituteInfo", "DateOfEstablishment", c => c.DateTime(nullable: false));
            DropColumn("dbo.InstituteInfo", "DateOfBirth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InstituteInfo", "DateOfBirth", c => c.DateTime(nullable: false));
            DropColumn("dbo.InstituteInfo", "DateOfEstablishment");
        }
    }
}
