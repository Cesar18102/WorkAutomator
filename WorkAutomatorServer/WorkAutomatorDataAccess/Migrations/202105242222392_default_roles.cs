namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class default_roles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.role", "is_default", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.role", "is_default");
        }
    }
}
