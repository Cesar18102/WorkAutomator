namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nonlocatedpipelineitemsandstoragecells2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.pipeline_item", new[] { "manufactory_id" });
            AlterColumn("dbo.pipeline_item", "manufactory_id", c => c.Int());
            CreateIndex("dbo.pipeline_item", "manufactory_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.pipeline_item", new[] { "manufactory_id" });
            AlterColumn("dbo.pipeline_item", "manufactory_id", c => c.Int(nullable: false));
            CreateIndex("dbo.pipeline_item", "manufactory_id");
        }
    }
}
