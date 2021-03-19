namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinancialYear_and_TaxGenerateInfo_tbl_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FinancialYear",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        YearName = c.String(maxLength: 200),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaxGenerateInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HoldingNo = c.String(maxLength: 200),
                        HouseOwnerId = c.Int(nullable: false),
                        UnionId = c.Int(),
                        FinancialYearId = c.Int(nullable: false),
                        TaxPercentage = c.Double(nullable: false),
                        TotalTax = c.Double(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.FinancialYear", t => t.FinancialYearId, cascadeDelete: true)
                .ForeignKey("dbo.HouseOwners", t => t.HouseOwnerId, cascadeDelete: true)
                .ForeignKey("dbo.UnionParishad", t => t.UnionId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.HouseOwnerId)
                .Index(t => t.UnionId)
                .Index(t => t.FinancialYearId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaxGenerateInfo", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.TaxGenerateInfo", "UnionId", "dbo.UnionParishad");
            DropForeignKey("dbo.TaxGenerateInfo", "HouseOwnerId", "dbo.HouseOwners");
            DropForeignKey("dbo.TaxGenerateInfo", "FinancialYearId", "dbo.FinancialYear");
            DropForeignKey("dbo.TaxGenerateInfo", "CreatedBy", "dbo.Users");
            DropIndex("dbo.TaxGenerateInfo", new[] { "UpdatedBy" });
            DropIndex("dbo.TaxGenerateInfo", new[] { "CreatedBy" });
            DropIndex("dbo.TaxGenerateInfo", new[] { "FinancialYearId" });
            DropIndex("dbo.TaxGenerateInfo", new[] { "UnionId" });
            DropIndex("dbo.TaxGenerateInfo", new[] { "HouseOwnerId" });
            DropTable("dbo.TaxGenerateInfo");
            DropTable("dbo.FinancialYear");
        }
    }
}
