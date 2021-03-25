namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstituteOffice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InstituteInfo", "WardInfoId", c => c.Int());
            AddColumn("dbo.InstituteInfo", "VillageInfoId", c => c.Int(nullable: false));
            AddColumn("dbo.InstituteInfo", "HoldingNo", c => c.String());
            AddColumn("dbo.InstituteInfo", "YearlyIncome", c => c.Double(nullable: false));
            AddColumn("dbo.InstituteInfo", "NameOfInstituteBangla", c => c.String());
            AddColumn("dbo.InstituteInfo", "NameOfInstituteEnglish", c => c.String());
            AddColumn("dbo.InstituteInfo", "MobileNo", c => c.String());
            AddColumn("dbo.InstituteInfo", "DateOfBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.InstituteInfo", "IsTubeWell", c => c.Boolean(nullable: false));
            AddColumn("dbo.InstituteInfo", "Sanitary", c => c.String());
            AddColumn("dbo.InstituteInfo", "TotalBuildingHouse", c => c.Int());
            AddColumn("dbo.InstituteInfo", "TotalSemiBuildingHouse", c => c.Int());
            AddColumn("dbo.InstituteInfo", "TotalRawHouse", c => c.Int());
            AddColumn("dbo.InstituteInfo", "InstituteType", c => c.String());
            AddColumn("dbo.InstituteInfo", "PreviousDueAmount", c => c.Double());
            CreateIndex("dbo.InstituteInfo", "WardInfoId");
            CreateIndex("dbo.InstituteInfo", "VillageInfoId");
            AddForeignKey("dbo.InstituteInfo", "VillageInfoId", "dbo.VillageInfo", "Id", cascadeDelete: true);
            AddForeignKey("dbo.InstituteInfo", "WardInfoId", "dbo.WardInfo", "Id");
            DropColumn("dbo.InstituteInfo", "NameOfInstitute");
            DropColumn("dbo.InstituteInfo", "Code");
            DropColumn("dbo.InstituteInfo", "EiinNumber");
            DropColumn("dbo.InstituteInfo", "TinNumber");
            DropColumn("dbo.InstituteInfo", "BinNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InstituteInfo", "BinNumber", c => c.String());
            AddColumn("dbo.InstituteInfo", "TinNumber", c => c.String());
            AddColumn("dbo.InstituteInfo", "EiinNumber", c => c.String());
            AddColumn("dbo.InstituteInfo", "Code", c => c.String());
            AddColumn("dbo.InstituteInfo", "NameOfInstitute", c => c.String());
            DropForeignKey("dbo.InstituteInfo", "WardInfoId", "dbo.WardInfo");
            DropForeignKey("dbo.InstituteInfo", "VillageInfoId", "dbo.VillageInfo");
            DropIndex("dbo.InstituteInfo", new[] { "VillageInfoId" });
            DropIndex("dbo.InstituteInfo", new[] { "WardInfoId" });
            DropColumn("dbo.InstituteInfo", "PreviousDueAmount");
            DropColumn("dbo.InstituteInfo", "InstituteType");
            DropColumn("dbo.InstituteInfo", "TotalRawHouse");
            DropColumn("dbo.InstituteInfo", "TotalSemiBuildingHouse");
            DropColumn("dbo.InstituteInfo", "TotalBuildingHouse");
            DropColumn("dbo.InstituteInfo", "Sanitary");
            DropColumn("dbo.InstituteInfo", "IsTubeWell");
            DropColumn("dbo.InstituteInfo", "DateOfBirth");
            DropColumn("dbo.InstituteInfo", "MobileNo");
            DropColumn("dbo.InstituteInfo", "NameOfInstituteEnglish");
            DropColumn("dbo.InstituteInfo", "NameOfInstituteBangla");
            DropColumn("dbo.InstituteInfo", "YearlyIncome");
            DropColumn("dbo.InstituteInfo", "HoldingNo");
            DropColumn("dbo.InstituteInfo", "VillageInfoId");
            DropColumn("dbo.InstituteInfo", "WardInfoId");
        }
    }
}
