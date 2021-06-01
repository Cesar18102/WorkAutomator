namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class storagecellspipeline : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.storage_cell", "pipeline_id", c => c.Int());
            CreateIndex("dbo.storage_cell", "pipeline_id");
            AddForeignKey("dbo.storage_cell", "pipeline_id", "dbo.pipeline", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.storage_cell", "pipeline_id", "dbo.pipeline");
            DropIndex("dbo.storage_cell", new[] { "pipeline_id" });
            DropColumn("dbo.storage_cell", "pipeline_id");
        }
    }
}
