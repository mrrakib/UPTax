namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfessionSocialBeneifitVillage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfessionInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProfessionTitle = c.String(nullable: false),
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
                "dbo.SocialBenefits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
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
                "dbo.VillageInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VillageName = c.String(nullable: false),
                        UnionId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.UnionParishad", t => t.UnionId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.UnionId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VillageInfo", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.VillageInfo", "UnionId", "dbo.UnionParishad");
            DropForeignKey("dbo.VillageInfo", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.SocialBenefits", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.SocialBenefits", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.ProfessionInfo", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.ProfessionInfo", "CreatedBy", "dbo.Users");
            DropIndex("dbo.VillageInfo", new[] { "UpdatedBy" });
            DropIndex("dbo.VillageInfo", new[] { "CreatedBy" });
            DropIndex("dbo.VillageInfo", new[] { "UnionId" });
            DropIndex("dbo.SocialBenefits", new[] { "UpdatedBy" });
            DropIndex("dbo.SocialBenefits", new[] { "CreatedBy" });
            DropIndex("dbo.ProfessionInfo", new[] { "UpdatedBy" });
            DropIndex("dbo.ProfessionInfo", new[] { "CreatedBy" });
            DropTable("dbo.VillageInfo");
            DropTable("dbo.SocialBenefits");
            DropTable("dbo.ProfessionInfo");
        }
    }
}
