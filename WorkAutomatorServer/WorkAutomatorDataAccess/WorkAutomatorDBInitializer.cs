using System.Data.Entity;
using WorkAutomatorDataAccess.Entities;

namespace WorkAutomatorDataAccess
{
    internal class WorkAutomatorDBInitializer : DropCreateDatabaseIfModelChanges<WorkAutomatorDBContext>
    {
        protected override void Seed(WorkAutomatorDBContext context)
        {
            context.VisualizerType.Add(new VisualizerTypeEntity() { name = "Signal" });
            context.VisualizerType.Add(new VisualizerTypeEntity() { name = "Value" });

            context.DataType.Add(new DataTypeEntity() { name = "int" });
            context.DataType.Add(new DataTypeEntity() { name = "float" });
            context.DataType.Add(new DataTypeEntity() { name = "bool" });
            context.DataType.Add(new DataTypeEntity() { name = "int[]" });
            context.DataType.Add(new DataTypeEntity() { name = "float[]" });
            context.DataType.Add(new DataTypeEntity() { name = "bool[]" });

            context.DbPermissionType.Add(new DbPermissionTypeEntity() { name = "READ" });
            context.DbPermissionType.Add(new DbPermissionTypeEntity() { name = "CREATE" });
            context.DbPermissionType.Add(new DbPermissionTypeEntity() { name = "UPDATE" });
            context.DbPermissionType.Add(new DbPermissionTypeEntity() { name = "DELETE" });

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
