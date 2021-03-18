namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropUnionId_from_infrastructuralType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InfraStructuralType", "UnionId", "dbo.UnionParishad");
            DropIndex("dbo.InfraStructuralType", new[] { "UnionId" });
            DropColumn("dbo.InfraStructuralType", "UnionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InfraStructuralType", "UnionId", c => c.Int(nullable: false));
            CreateIndex("dbo.InfraStructuralType", "UnionId");
            AddForeignKey("dbo.InfraStructuralType", "UnionId", "dbo.UnionParishad", "Id", cascadeDelete: true);
        }
    }
}
