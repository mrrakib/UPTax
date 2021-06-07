namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredMessage : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MessageInfo", "Message", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MessageInfo", "Message", c => c.String());
        }
    }
}
