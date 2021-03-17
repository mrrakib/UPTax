namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_tbl_InfraStructaralType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InfraStructuralType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(nullable: false, maxLength: 150),
                        YearlyRent = c.Double(nullable: false),
                        InterestRate = c.Double(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InfraStructuralType", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.InfraStructuralType", "CreatedBy", "dbo.Users");
            DropIndex("dbo.InfraStructuralType", new[] { "UpdatedBy" });
            DropIndex("dbo.InfraStructuralType", new[] { "CreatedBy" });
            DropTable("dbo.InfraStructuralType");
        }
    }
}
