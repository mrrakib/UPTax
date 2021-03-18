namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialHouseOwner : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
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
                "dbo.HouseOwners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerNameInEnglish = c.String(),
                        OwnerNameInBangla = c.String(),
                        YearlyIncome = c.Double(nullable: false),
                        WardInfoId = c.Int(nullable: false),
                        VillageInfoId = c.Int(nullable: false),
                        HoldingNo = c.String(),
                        EducationInfoId = c.Int(nullable: false),
                        MobileNo = c.String(),
                        FatherHusbandName = c.String(),
                        MotherName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        BirthRegistrationNumber = c.String(),
                        NIDNumber = c.String(),
                        GenderId = c.Int(nullable: false),
                        ReligionId = c.Int(nullable: false),
                        ProfessionId = c.Int(nullable: false),
                        TubeWell = c.Boolean(nullable: false),
                        Sanitary = c.String(),
                        SocialBenefitBeforeId = c.Int(),
                        SocialBenefitEligibleId = c.Int(),
                        SocialBenefitRunningId = c.Int(),
                        InfrastructureTypeId = c.Int(nullable: false),
                        YearlyRentAmount = c.Double(),
                        YearlyLoanAmount = c.Double(),
                        LivingType = c.String(),
                        PreviousDueAmount = c.Double(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.EducationInfo", t => t.EducationInfoId, cascadeDelete: true)
                .ForeignKey("dbo.Genders", t => t.GenderId, cascadeDelete: true)
                .ForeignKey("dbo.InfrastructureInfo", t => t.InfrastructureTypeId, cascadeDelete: true)
                .ForeignKey("dbo.ProfessionInfo", t => t.ProfessionId, cascadeDelete: true)
                .ForeignKey("dbo.Religions", t => t.ReligionId, cascadeDelete: true)
                .ForeignKey("dbo.SocialBenefits", t => t.SocialBenefitBeforeId)
                .ForeignKey("dbo.SocialBenefits", t => t.SocialBenefitEligibleId)
                .ForeignKey("dbo.SocialBenefits", t => t.SocialBenefitRunningId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .ForeignKey("dbo.VillageInfo", t => t.VillageInfoId, cascadeDelete: true)
                .Index(t => t.VillageInfoId)
                .Index(t => t.EducationInfoId)
                .Index(t => t.GenderId)
                .Index(t => t.ReligionId)
                .Index(t => t.ProfessionId)
                .Index(t => t.SocialBenefitBeforeId)
                .Index(t => t.SocialBenefitEligibleId)
                .Index(t => t.SocialBenefitRunningId)
                .Index(t => t.InfrastructureTypeId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.Religions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
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
            DropForeignKey("dbo.HouseOwners", "VillageInfoId", "dbo.VillageInfo");
            DropForeignKey("dbo.HouseOwners", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.HouseOwners", "SocialBenefitRunningId", "dbo.SocialBenefits");
            DropForeignKey("dbo.HouseOwners", "SocialBenefitEligibleId", "dbo.SocialBenefits");
            DropForeignKey("dbo.HouseOwners", "SocialBenefitBeforeId", "dbo.SocialBenefits");
            DropForeignKey("dbo.HouseOwners", "ReligionId", "dbo.Religions");
            DropForeignKey("dbo.Religions", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Religions", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.HouseOwners", "ProfessionId", "dbo.ProfessionInfo");
            DropForeignKey("dbo.HouseOwners", "InfrastructureTypeId", "dbo.InfrastructureInfo");
            DropForeignKey("dbo.HouseOwners", "GenderId", "dbo.Genders");
            DropForeignKey("dbo.HouseOwners", "EducationInfoId", "dbo.EducationInfo");
            DropForeignKey("dbo.HouseOwners", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Genders", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Genders", "CreatedBy", "dbo.Users");
            DropIndex("dbo.Religions", new[] { "UpdatedBy" });
            DropIndex("dbo.Religions", new[] { "CreatedBy" });
            DropIndex("dbo.HouseOwners", new[] { "UpdatedBy" });
            DropIndex("dbo.HouseOwners", new[] { "CreatedBy" });
            DropIndex("dbo.HouseOwners", new[] { "InfrastructureTypeId" });
            DropIndex("dbo.HouseOwners", new[] { "SocialBenefitRunningId" });
            DropIndex("dbo.HouseOwners", new[] { "SocialBenefitEligibleId" });
            DropIndex("dbo.HouseOwners", new[] { "SocialBenefitBeforeId" });
            DropIndex("dbo.HouseOwners", new[] { "ProfessionId" });
            DropIndex("dbo.HouseOwners", new[] { "ReligionId" });
            DropIndex("dbo.HouseOwners", new[] { "GenderId" });
            DropIndex("dbo.HouseOwners", new[] { "EducationInfoId" });
            DropIndex("dbo.HouseOwners", new[] { "VillageInfoId" });
            DropIndex("dbo.Genders", new[] { "UpdatedBy" });
            DropIndex("dbo.Genders", new[] { "CreatedBy" });
            DropTable("dbo.Religions");
            DropTable("dbo.HouseOwners");
            DropTable("dbo.Genders");
        }
    }
}
