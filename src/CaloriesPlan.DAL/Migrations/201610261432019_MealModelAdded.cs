namespace CaloriesPlan.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MealModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Meal",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 200),
                        Calories = c.Int(nullable: false),
                        EatingDate = c.DateTime(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Meal", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Meal", new[] { "UserID" });
            DropTable("dbo.Meal");
        }
    }
}
