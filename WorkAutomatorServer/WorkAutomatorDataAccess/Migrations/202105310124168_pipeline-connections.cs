namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pipelineconnections : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.pipeline_item_connection", "pipeline_item1_id", "dbo.pipeline_item");
            DropForeignKey("dbo.pipeline_item_connection", "pipeline_item2_id", "dbo.pipeline_item");
            DropForeignKey("dbo.pipeline_item_storage_connection", "storage_cell_id", "dbo.storage_cell");
            DropForeignKey("dbo.pipeline_item_storage_connection", "pipeline_item_id", "dbo.pipeline_item");
            DropIndex("dbo.pipeline_item_connection", new[] { "pipeline_item1_id" });
            DropIndex("dbo.pipeline_item_connection", new[] { "pipeline_item2_id" });
            DropIndex("dbo.pipeline_item_storage_connection", new[] { "pipeline_item_id" });
            DropIndex("dbo.pipeline_item_storage_connection", new[] { "storage_cell_id" });
            CreateTable(
                "dbo.pipeline_item_pipeline_item_connection",
                c => new
                    {
                        output_pipeline_item_id = c.Int(nullable: false),
                        input_pipeline_item_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.output_pipeline_item_id, t.input_pipeline_item_id })
                .ForeignKey("dbo.pipeline_item", t => t.output_pipeline_item_id)
                .ForeignKey("dbo.pipeline_item", t => t.input_pipeline_item_id)
                .Index(t => t.output_pipeline_item_id)
                .Index(t => t.input_pipeline_item_id);
            
            CreateTable(
                "dbo.input_storage_cell_connection",
                c => new
                    {
                        output_pipeline_item_id = c.Int(nullable: false),
                        input_storage_cell_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.output_pipeline_item_id, t.input_storage_cell_id })
                .ForeignKey("dbo.pipeline_item", t => t.output_pipeline_item_id, cascadeDelete: true)
                .ForeignKey("dbo.storage_cell", t => t.input_storage_cell_id, cascadeDelete: true)
                .Index(t => t.output_pipeline_item_id)
                .Index(t => t.input_storage_cell_id);
            
            CreateTable(
                "dbo.output_storage_cell_connection",
                c => new
                    {
                        input_pipeline_item_id = c.Int(nullable: false),
                        output_storage_cell_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.input_pipeline_item_id, t.output_storage_cell_id })
                .ForeignKey("dbo.pipeline_item", t => t.input_pipeline_item_id, cascadeDelete: true)
                .ForeignKey("dbo.storage_cell", t => t.output_storage_cell_id, cascadeDelete: true)
                .Index(t => t.input_pipeline_item_id)
                .Index(t => t.output_storage_cell_id);
            
            DropTable("dbo.pipeline_item_connection");
            DropTable("dbo.pipeline_item_storage_connection");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.pipeline_item_storage_connection",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pipeline_item_id = c.Int(nullable: false),
                        storage_cell_id = c.Int(nullable: false),
                        is_direct = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.pipeline_item_connection",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pipeline_item1_id = c.Int(nullable: false),
                        pipeline_item2_id = c.Int(nullable: false),
                        is_direct = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            DropForeignKey("dbo.output_storage_cell_connection", "output_storage_cell_id", "dbo.storage_cell");
            DropForeignKey("dbo.output_storage_cell_connection", "input_pipeline_item_id", "dbo.pipeline_item");
            DropForeignKey("dbo.input_storage_cell_connection", "input_storage_cell_id", "dbo.storage_cell");
            DropForeignKey("dbo.input_storage_cell_connection", "output_pipeline_item_id", "dbo.pipeline_item");
            DropForeignKey("dbo.pipeline_item_pipeline_item_connection", "input_pipeline_item_id", "dbo.pipeline_item");
            DropForeignKey("dbo.pipeline_item_pipeline_item_connection", "output_pipeline_item_id", "dbo.pipeline_item");
            DropIndex("dbo.output_storage_cell_connection", new[] { "output_storage_cell_id" });
            DropIndex("dbo.output_storage_cell_connection", new[] { "input_pipeline_item_id" });
            DropIndex("dbo.input_storage_cell_connection", new[] { "input_storage_cell_id" });
            DropIndex("dbo.input_storage_cell_connection", new[] { "output_pipeline_item_id" });
            DropIndex("dbo.pipeline_item_pipeline_item_connection", new[] { "input_pipeline_item_id" });
            DropIndex("dbo.pipeline_item_pipeline_item_connection", new[] { "output_pipeline_item_id" });
            DropTable("dbo.output_storage_cell_connection");
            DropTable("dbo.input_storage_cell_connection");
            DropTable("dbo.pipeline_item_pipeline_item_connection");
            CreateIndex("dbo.pipeline_item_storage_connection", "storage_cell_id");
            CreateIndex("dbo.pipeline_item_storage_connection", "pipeline_item_id");
            CreateIndex("dbo.pipeline_item_connection", "pipeline_item2_id");
            CreateIndex("dbo.pipeline_item_connection", "pipeline_item1_id");
            AddForeignKey("dbo.pipeline_item_storage_connection", "pipeline_item_id", "dbo.pipeline_item", "id");
            AddForeignKey("dbo.pipeline_item_storage_connection", "storage_cell_id", "dbo.storage_cell", "id");
            AddForeignKey("dbo.pipeline_item_connection", "pipeline_item2_id", "dbo.pipeline_item", "id");
            AddForeignKey("dbo.pipeline_item_connection", "pipeline_item1_id", "dbo.pipeline_item", "id");
        }
    }
}
