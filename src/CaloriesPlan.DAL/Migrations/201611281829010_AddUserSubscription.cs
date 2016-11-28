namespace CaloriesPlan.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserSubscription : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSubscription",
                c => new
                    {
                        SubscriberID = c.String(nullable: false, maxLength: 128),
                        UserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SubscriberID, t.UserID })
                .ForeignKey("dbo.AspNetUsers", t => t.SubscriberID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: false)
                .Index(t => t.SubscriberID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSubscription", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserSubscription", "SubscriberID", "dbo.AspNetUsers");
            DropIndex("dbo.UserSubscription", new[] { "UserID" });
            DropIndex("dbo.UserSubscription", new[] { "SubscriberID" });
            DropTable("dbo.UserSubscription");
        }
    }
}
