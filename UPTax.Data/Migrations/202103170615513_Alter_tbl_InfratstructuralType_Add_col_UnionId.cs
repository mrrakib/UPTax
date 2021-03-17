namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_tbl_InfratstructuralType_Add_col_UnionId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InfraStructuralType", "UnionId", c => c.Int(nullable: false));
            CreateIndex("dbo.InfraStructuralType", "UnionId");
            AddForeignKey("dbo.InfraStructuralType", "UnionId", "dbo.UnionParishad", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InfraStructuralType", "UnionId", "dbo.UnionParishad");
            DropIndex("dbo.InfraStructuralType", new[] { "UnionId" });
            DropColumn("dbo.InfraStructuralType", "UnionId");
        }
    }
}
