namespace WorkAutomatorLogic.Models.Permission
{
    public interface IValueConverter
    {
        object Convert(object value);
    }

    public class PermissionModelBaseToTableNameConverter : IValueConverter
    {
        public object Convert(object value)
        {
            PermissionModelBase model = value as PermissionModelBase;

            if (model is PermissionDbModel dbPermission)
                return DbTable.RoleDbPermission;
            else if (model is PermissionModel commonPermission)
            {
                switch (commonPermission.InteractionType)
                {
                    case InteractionType.DETECTOR: return DbTable.RoleDetectorPermission;
                    case InteractionType.MANUFACTORY: return DbTable.RoleManufactoryPermission;
                    case InteractionType.PIPELINE_ITEM: return DbTable.RolePipelineItemPermission;
                    case InteractionType.STORAGE: return DbTable.RoleStorageCellPermission;
                }
            }

            return null;
        }
    }
}
