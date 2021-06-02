namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class faultassociatedtask : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.detector_fault_event");
            AddColumn("dbo.task", "is_done", c => c.Boolean(nullable: false));
            AddColumn("dbo.detector_fault_event", "associated_task_id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.detector_fault_event", "associated_task_id");
            CreateIndex("dbo.detector_fault_event", "associated_task_id");
            AddForeignKey("dbo.detector_fault_event", "associated_task_id", "dbo.task", "id");
            DropColumn("dbo.detector_fault_event", "id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.detector_fault_event", "id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.detector_fault_event", "associated_task_id", "dbo.task");
            DropIndex("dbo.detector_fault_event", new[] { "associated_task_id" });
            DropPrimaryKey("dbo.detector_fault_event");
            DropColumn("dbo.detector_fault_event", "associated_task_id");
            DropColumn("dbo.task", "is_done");
            AddPrimaryKey("dbo.detector_fault_event", "id");
        }
    }
}
