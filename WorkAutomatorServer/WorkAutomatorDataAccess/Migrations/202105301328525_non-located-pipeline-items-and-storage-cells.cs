namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nonlocatedpipelineitemsandstoragecells : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.pipeline_item", "x", c => c.Double());
            AlterColumn("dbo.pipeline_item", "y", c => c.Double());
            AlterColumn("dbo.storage_cell", "x", c => c.Double());
            AlterColumn("dbo.storage_cell", "y", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.storage_cell", "y", c => c.Double(nullable: false));
            AlterColumn("dbo.storage_cell", "x", c => c.Double(nullable: false));
            AlterColumn("dbo.pipeline_item", "y", c => c.Double(nullable: false));
            AlterColumn("dbo.pipeline_item", "x", c => c.Double(nullable: false));
        }
    }
}
