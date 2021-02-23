namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_tbl_UnionParishad : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UnionParishad",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        PhoneNo = c.String(maxLength: 14),
                        Email = c.String(maxLength: 150),
                        Description = c.String(maxLength: 350),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UnionParishad");
        }
    }
}
