namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WardInfos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WardInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WardNo = c.String(),
                        UnionId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WardInfo");
        }
    }
}
