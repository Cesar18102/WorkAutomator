namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cascades : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.task", new[] { "assignee_account_id" });
            DropIndex("dbo.task", new[] { "reviewer_account_id" });
            DropIndex("dbo.detector", new[] { "pipeline_item_id" });
            DropIndex("dbo.pipeline_item", new[] { "pipeline_id" });
            DropIndex("dbo.storage_cell", new[] { "manufactory_id" });
            DropIndex("dbo.resource", new[] { "unit_id" });
            AlterColumn("dbo.task", "assignee_account_id", c => c.Int());
            AlterColumn("dbo.task", "reviewer_account_id", c => c.Int());
            AlterColumn("dbo.detector", "pipeline_item_id", c => c.Int());
            AlterColumn("dbo.pipeline_item", "pipeline_id", c => c.Int());
            AlterColumn("dbo.storage_cell", "manufactory_id", c => c.Int());
            AlterColumn("dbo.resource", "unit_id", c => c.Int());
            CreateIndex("dbo.task", "assignee_account_id");
            CreateIndex("dbo.task", "reviewer_account_id");
            CreateIndex("dbo.detector", "pipeline_item_id");
            CreateIndex("dbo.pipeline_item", "pipeline_id");
            CreateIndex("dbo.storage_cell", "manufactory_id");
            CreateIndex("dbo.resource", "unit_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.resource", new[] { "unit_id" });
            DropIndex("dbo.storage_cell", new[] { "manufactory_id" });
            DropIndex("dbo.pipeline_item", new[] { "pipeline_id" });
            DropIndex("dbo.detector", new[] { "pipeline_item_id" });
            DropIndex("dbo.task", new[] { "reviewer_account_id" });
            DropIndex("dbo.task", new[] { "assignee_account_id" });
            AlterColumn("dbo.resource", "unit_id", c => c.Int(nullable: false));
            AlterColumn("dbo.storage_cell", "manufactory_id", c => c.Int(nullable: false));
            AlterColumn("dbo.pipeline_item", "pipeline_id", c => c.Int(nullable: false));
            AlterColumn("dbo.detector", "pipeline_item_id", c => c.Int(nullable: false));
            AlterColumn("dbo.task", "reviewer_account_id", c => c.Int(nullable: false));
            AlterColumn("dbo.task", "assignee_account_id", c => c.Int(nullable: false));
            CreateIndex("dbo.resource", "unit_id");
            CreateIndex("dbo.storage_cell", "manufactory_id");
            CreateIndex("dbo.pipeline_item", "pipeline_id");
            CreateIndex("dbo.detector", "pipeline_item_id");
            CreateIndex("dbo.task", "reviewer_account_id");
            CreateIndex("dbo.task", "assignee_account_id");
        }
    }
}
