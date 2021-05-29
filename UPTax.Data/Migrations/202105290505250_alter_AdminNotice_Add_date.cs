namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_AdminNotice_Add_date : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdminNotice", "FromDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AdminNotice", "ToDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdminNotice", "ToDate");
            DropColumn("dbo.AdminNotice", "FromDate");
        }
    }
}
