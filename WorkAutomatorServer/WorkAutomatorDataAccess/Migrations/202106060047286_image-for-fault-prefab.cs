namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imageforfaultprefab : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.detector_fault_prefab", "image_url", c => c.String(nullable: false, maxLength: 1024));
        }
        
        public override void Down()
        {
            DropColumn("dbo.detector_fault_prefab", "image_url");
        }
    }
}
