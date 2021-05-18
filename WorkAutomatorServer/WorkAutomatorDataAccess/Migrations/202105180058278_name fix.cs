namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class namefix : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DetectorPrefabEntities", newName: "detector_prefab");
            RenameTable(name: "dbo.DetectorInteractionEventEntities", newName: "detector_interaction_event");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.detector_interaction_event", newName: "DetectorInteractionEventEntities");
            RenameTable(name: "dbo.detector_prefab", newName: "DetectorPrefabEntities");
        }
    }
}
