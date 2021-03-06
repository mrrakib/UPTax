namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InfrastructureInstuates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InfrastructureInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HoldingNo = c.String(),
                        TypeOfInfrastructure = c.String(),
                        TotalNumberOfHouse = c.Int(nullable: false),
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
            
            CreateTable(
                "dbo.InstituteInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameOfInstitute = c.String(),
                        EiinNumber = c.String(),
                        TinNumber = c.String(),
                        BinNumber = c.String(),
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
            DropForeignKey("dbo.InstituteInfo", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.InstituteInfo", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.InfrastructureInfo", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.InfrastructureInfo", "CreatedBy", "dbo.Users");
            DropIndex("dbo.InstituteInfo", new[] { "UpdatedBy" });
            DropIndex("dbo.InstituteInfo", new[] { "CreatedBy" });
            DropIndex("dbo.InfrastructureInfo", new[] { "UpdatedBy" });
            DropIndex("dbo.InfrastructureInfo", new[] { "CreatedBy" });
            DropTable("dbo.InstituteInfo");
            DropTable("dbo.InfrastructureInfo");
        }
    }
}
