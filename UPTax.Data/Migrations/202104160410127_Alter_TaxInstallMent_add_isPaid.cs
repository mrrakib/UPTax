namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_TaxInstallMent_add_isPaid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaxInstallments", "IsPaid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaxInstallments", "IsPaid");
        }
    }
}
