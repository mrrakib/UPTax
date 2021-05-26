namespace UPTax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageReplyMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessageReply",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageInfoId = c.Int(nullable: false),
                        ReplyMessage = c.String(),
                        ReplyerUserId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ReplyerUserId)
                .Index(t => t.ReplyerUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MessageReply", "ReplyerUserId", "dbo.Users");
            DropIndex("dbo.MessageReply", new[] { "ReplyerUserId" });
            DropTable("dbo.MessageReply");
        }
    }
}
