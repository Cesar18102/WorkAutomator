namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class superadmin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.account", "is_superadmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.account", "is_superadmin");
        }
    }
}
