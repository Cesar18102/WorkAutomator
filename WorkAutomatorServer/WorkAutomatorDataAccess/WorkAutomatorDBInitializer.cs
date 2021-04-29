using System.Data.Entity;

namespace WorkAutomatorDataAccess.DataContext
{
    internal class WorkAutomatorDBInitializer : DropCreateDatabaseIfModelChanges<WorkAutomatorDBContext>
    {
        protected override void Seed(WorkAutomatorDBContext context)
        {
            base.Seed(context);
        }
    }
}
