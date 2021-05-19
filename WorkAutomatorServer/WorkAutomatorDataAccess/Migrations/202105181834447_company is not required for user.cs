namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class companyisnotrequiredforuser : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.account", new[] { "company_id" });
            AlterColumn("dbo.account", "company_id", c => c.Int());
            CreateIndex("dbo.account", "company_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.account", new[] { "company_id" });
            AlterColumn("dbo.account", "company_id", c => c.Int(nullable: false));
            CreateIndex("dbo.account", "company_id");
        }
    }
}
