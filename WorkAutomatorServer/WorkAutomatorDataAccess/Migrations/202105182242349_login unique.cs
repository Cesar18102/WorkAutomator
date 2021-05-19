namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class loginunique : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.account", "login", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.account", new[] { "login" });
        }
    }
}
