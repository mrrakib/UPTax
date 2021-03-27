namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelationshipsMember : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HoldingNo = c.String(),
                        MemberNameInBangla = c.String(),
                        ProfessionId = c.Int(),
                        DateOfBirth = c.DateTime(nullable: false),
                        BirthRegistrationNumber = c.String(),
                        NIDNumber = c.String(),
                        GenderId = c.Int(nullable: false),
                        RelationshipId = c.Int(nullable: false),
                        EducationInfoId = c.Int(),
                        SocialBenefitBeforeId = c.Int(),
                        SocialBenefitEligibleId = c.Int(),
                        SocialBenefitRunningId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.EducationInfo", t => t.EducationInfoId)
                .ForeignKey("dbo.Genders", t => t.GenderId, cascadeDelete: true)
                .ForeignKey("dbo.ProfessionInfo", t => t.ProfessionId)
                .ForeignKey("dbo.Relationships", t => t.RelationshipId, cascadeDelete: true)
                .ForeignKey("dbo.SocialBenefits", t => t.SocialBenefitBeforeId)
                .ForeignKey("dbo.SocialBenefits", t => t.SocialBenefitEligibleId)
                .ForeignKey("dbo.SocialBenefits", t => t.SocialBenefitRunningId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.ProfessionId)
                .Index(t => t.GenderId)
                .Index(t => t.RelationshipId)
                .Index(t => t.EducationInfoId)
                .Index(t => t.SocialBenefitBeforeId)
                .Index(t => t.SocialBenefitEligibleId)
                .Index(t => t.SocialBenefitRunningId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.Relationships",
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
            DropForeignKey("dbo.Members", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Members", "SocialBenefitRunningId", "dbo.SocialBenefits");
            DropForeignKey("dbo.Members", "SocialBenefitEligibleId", "dbo.SocialBenefits");
            DropForeignKey("dbo.Members", "SocialBenefitBeforeId", "dbo.SocialBenefits");
            DropForeignKey("dbo.Members", "RelationshipId", "dbo.Relationships");
            DropForeignKey("dbo.Relationships", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Relationships", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Members", "ProfessionId", "dbo.ProfessionInfo");
            DropForeignKey("dbo.Members", "GenderId", "dbo.Genders");
            DropForeignKey("dbo.Members", "EducationInfoId", "dbo.EducationInfo");
            DropForeignKey("dbo.Members", "CreatedBy", "dbo.Users");
            DropIndex("dbo.Relationships", new[] { "UpdatedBy" });
            DropIndex("dbo.Relationships", new[] { "CreatedBy" });
            DropIndex("dbo.Members", new[] { "UpdatedBy" });
            DropIndex("dbo.Members", new[] { "CreatedBy" });
            DropIndex("dbo.Members", new[] { "SocialBenefitRunningId" });
            DropIndex("dbo.Members", new[] { "SocialBenefitEligibleId" });
            DropIndex("dbo.Members", new[] { "SocialBenefitBeforeId" });
            DropIndex("dbo.Members", new[] { "EducationInfoId" });
            DropIndex("dbo.Members", new[] { "RelationshipId" });
            DropIndex("dbo.Members", new[] { "GenderId" });
            DropIndex("dbo.Members", new[] { "ProfessionId" });
            DropTable("dbo.Relationships");
            DropTable("dbo.Members");
        }
    }
}
