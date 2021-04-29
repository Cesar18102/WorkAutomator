using System.Data.Entity.Migrations;

namespace WorkAutomatorDataAccess.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<WorkAutomatorDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WorkAutomatorDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
