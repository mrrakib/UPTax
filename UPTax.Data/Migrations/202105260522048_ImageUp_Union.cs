namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageUp_Union : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UnionParishad", "ImagePath", c => c.String());
            AddColumn("dbo.UnionParishad", "ImageName", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UnionParishad", "ImageName");
            DropColumn("dbo.UnionParishad", "ImagePath");
        }
    }
}
