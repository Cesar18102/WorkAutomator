using System.Data.Entity;

namespace WorkAutomatorDataAccess
{
    internal class WorkAutomatorDBInitializer : DropCreateDatabaseIfModelChanges<WorkAutomatorDBContext>
    {
        protected override void Seed(WorkAutomatorDBContext context)
        {
            base.Seed(context);
        }
    }
}
