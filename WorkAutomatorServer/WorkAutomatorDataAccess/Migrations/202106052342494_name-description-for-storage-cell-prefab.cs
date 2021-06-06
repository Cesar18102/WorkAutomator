namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class namedescriptionforstoragecellprefab : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.storage_cell_prefab", "name", c => c.String(nullable: false, maxLength: 1024, unicode: false));
            AddColumn("dbo.storage_cell_prefab", "description", c => c.String(unicode: false, storeType: "text"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.storage_cell_prefab", "description");
            DropColumn("dbo.storage_cell_prefab", "name");
        }
    }
}
