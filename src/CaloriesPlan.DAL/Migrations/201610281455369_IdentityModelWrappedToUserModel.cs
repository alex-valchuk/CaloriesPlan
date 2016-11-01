namespace CaloriesPlan.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityModelWrappedToUserModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DailyCaloriesLimit", c => c.Int(nullable: false, defaultValue: 50));
        }

        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DailyCaloriesLimit");
        }
    }
}
