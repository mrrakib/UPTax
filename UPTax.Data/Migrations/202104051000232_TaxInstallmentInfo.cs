namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxInstallmentInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaxInstallments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnionId = c.Int(),
                        WardInfoId = c.Int(nullable: false),
                        FinancialYearId = c.Int(nullable: false),
                        HoldingNo = c.String(),
                        TaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OutstandingAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PenaltyAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxPaymentDate = c.DateTime(nullable: false),
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
            DropForeignKey("dbo.TaxInstallments", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.TaxInstallments", "CreatedBy", "dbo.Users");
            DropIndex("dbo.TaxInstallments", new[] { "UpdatedBy" });
            DropIndex("dbo.TaxInstallments", new[] { "CreatedBy" });
            DropTable("dbo.TaxInstallments");
        }
    }
}
