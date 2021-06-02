namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isreviewedtask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.task", "is_reviewed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.task", "is_reviewed");
        }
    }
}
