namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_unionId_in_user_tbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UnionId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "UnionId");
        }
    }
}
