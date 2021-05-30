namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class no_superadmin_field : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.account", "is_superadmin");
        }
        
        public override void Down()
        {
            AddColumn("dbo.account", "is_superadmin", c => c.Boolean(nullable: false));
        }
    }
}
