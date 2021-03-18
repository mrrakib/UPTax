namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HouseOwnersUpdateType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HouseOwners", "EducationInfoId", "dbo.EducationInfo");
            DropForeignKey("dbo.HouseOwners", "ProfessionId", "dbo.ProfessionInfo");
            DropIndex("dbo.HouseOwners", new[] { "EducationInfoId" });
            DropIndex("dbo.HouseOwners", new[] { "ProfessionId" });
            AlterColumn("dbo.HouseOwners", "EducationInfoId", c => c.Int());
            AlterColumn("dbo.HouseOwners", "ProfessionId", c => c.Int());
            CreateIndex("dbo.HouseOwners", "EducationInfoId");
            CreateIndex("dbo.HouseOwners", "ProfessionId");
            AddForeignKey("dbo.HouseOwners", "EducationInfoId", "dbo.EducationInfo", "Id");
            AddForeignKey("dbo.HouseOwners", "ProfessionId", "dbo.ProfessionInfo", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HouseOwners", "ProfessionId", "dbo.ProfessionInfo");
            DropForeignKey("dbo.HouseOwners", "EducationInfoId", "dbo.EducationInfo");
            DropIndex("dbo.HouseOwners", new[] { "ProfessionId" });
            DropIndex("dbo.HouseOwners", new[] { "EducationInfoId" });
            AlterColumn("dbo.HouseOwners", "ProfessionId", c => c.Int(nullable: false));
            AlterColumn("dbo.HouseOwners", "EducationInfoId", c => c.Int(nullable: false));
            CreateIndex("dbo.HouseOwners", "ProfessionId");
            CreateIndex("dbo.HouseOwners", "EducationInfoId");
            AddForeignKey("dbo.HouseOwners", "ProfessionId", "dbo.ProfessionInfo", "Id", cascadeDelete: true);
            AddForeignKey("dbo.HouseOwners", "EducationInfoId", "dbo.EducationInfo", "Id", cascadeDelete: true);
        }
    }
}
