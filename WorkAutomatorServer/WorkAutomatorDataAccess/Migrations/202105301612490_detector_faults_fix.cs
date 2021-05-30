namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class detector_faults_fix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.detector_fault_event", "detector_fault_id", "dbo.detector_fault");
            DropForeignKey("dbo.detector_fault", "detector_fault_prefab_id", "dbo.detector_fault_prefab");
            DropForeignKey("dbo.detector_fault", "detector_id", "dbo.detector");
            DropIndex("dbo.detector_fault", new[] { "detector_id" });
            DropIndex("dbo.detector_fault", new[] { "detector_fault_prefab_id" });
            DropIndex("dbo.detector_fault_event", new[] { "detector_fault_id" });
            CreateTable(
                "dbo.detector_tracked_fault_prefab",
                c => new
                    {
                        detector_id = c.Int(nullable: false),
                        detector_fault_prefab_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.detector_id, t.detector_fault_prefab_id })
                .ForeignKey("dbo.detector", t => t.detector_id, cascadeDelete: true)
                .ForeignKey("dbo.detector_fault_prefab", t => t.detector_fault_prefab_id, cascadeDelete: true)
                .Index(t => t.detector_id)
                .Index(t => t.detector_fault_prefab_id);
            
            AddColumn("dbo.detector_fault_event", "detector_id", c => c.Int(nullable: false));
            AddColumn("dbo.detector_fault_event", "detector_fault_prefab_id", c => c.Int(nullable: false));
            CreateIndex("dbo.detector_fault_event", "detector_id");
            CreateIndex("dbo.detector_fault_event", "detector_fault_prefab_id");
            AddForeignKey("dbo.detector_fault_event", "detector_fault_prefab_id", "dbo.detector_fault_prefab", "id", cascadeDelete: true);
            AddForeignKey("dbo.detector_fault_event", "detector_id", "dbo.detector", "id", cascadeDelete: true);
            DropColumn("dbo.detector_fault_event", "detector_fault_id");
            DropTable("dbo.detector_fault");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.detector_fault",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        detector_id = c.Int(nullable: false),
                        detector_fault_prefab_id = c.Int(nullable: false),
                        log = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.detector_fault_event", "detector_fault_id", c => c.Int(nullable: false));
            DropForeignKey("dbo.detector_tracked_fault_prefab", "detector_fault_prefab_id", "dbo.detector_fault_prefab");
            DropForeignKey("dbo.detector_tracked_fault_prefab", "detector_id", "dbo.detector");
            DropForeignKey("dbo.detector_fault_event", "detector_id", "dbo.detector");
            DropForeignKey("dbo.detector_fault_event", "detector_fault_prefab_id", "dbo.detector_fault_prefab");
            DropIndex("dbo.detector_tracked_fault_prefab", new[] { "detector_fault_prefab_id" });
            DropIndex("dbo.detector_tracked_fault_prefab", new[] { "detector_id" });
            DropIndex("dbo.detector_fault_event", new[] { "detector_fault_prefab_id" });
            DropIndex("dbo.detector_fault_event", new[] { "detector_id" });
            DropColumn("dbo.detector_fault_event", "detector_fault_prefab_id");
            DropColumn("dbo.detector_fault_event", "detector_id");
            DropTable("dbo.detector_tracked_fault_prefab");
            CreateIndex("dbo.detector_fault_event", "detector_fault_id");
            CreateIndex("dbo.detector_fault", "detector_fault_prefab_id");
            CreateIndex("dbo.detector_fault", "detector_id");
            AddForeignKey("dbo.detector_fault", "detector_id", "dbo.detector", "id");
            AddForeignKey("dbo.detector_fault", "detector_fault_prefab_id", "dbo.detector_fault_prefab", "id");
            AddForeignKey("dbo.detector_fault_event", "detector_fault_id", "dbo.detector_fault", "id");
        }
    }
}
