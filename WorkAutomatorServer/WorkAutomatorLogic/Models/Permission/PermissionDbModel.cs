using Constants;

namespace WorkAutomatorLogic.Models.Permission
{
    public class PermissionDbModel : PermissionModelBase
    {
        public InteractionDbType InteractionDbType { get; private set; }
        public DbTable DbTable { get; private set; }

        public PermissionDbModel(InteractionDbType interactionDbType, DbTable table) : base(InteractionType.DB)
        {
            InteractionDbType = interactionDbType;
            DbTable = table;
        }
    }
}
