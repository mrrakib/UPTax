namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_tbl_InfraStructuralType_add_col_StaticId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InfraStructuralType", "StaticId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InfraStructuralType", "StaticId");
        }
    }
}
