namespace CaloriesPlan.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PasswordSaltAddedTuUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PasswordSalt", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PasswordSalt");
        }
    }
}
