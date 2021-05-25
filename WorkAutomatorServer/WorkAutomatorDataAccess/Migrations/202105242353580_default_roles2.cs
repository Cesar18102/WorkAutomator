namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class default_roles2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.role", new[] { "company_id" });
            AlterColumn("dbo.role", "company_id", c => c.Int());
            CreateIndex("dbo.role", "company_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.role", new[] { "company_id" });
            AlterColumn("dbo.role", "company_id", c => c.Int(nullable: false));
            CreateIndex("dbo.role", "company_id");
        }
    }
}
