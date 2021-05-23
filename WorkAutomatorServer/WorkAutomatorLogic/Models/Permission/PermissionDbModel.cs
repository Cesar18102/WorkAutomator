namespace WorkAutomatorLogic.Models.Permission
{
    public enum DbTable
    {

    }

    public class PermissionDbModel : PermissionModelBase
    {
        public InteractionDbType InteractionDbType { get; private set; }
        public DbTable DbTable { get; private set; }

        public PermissionDbModel(InteractionDbType interactionDbType, DbTable table) : base(InteractionType.DB)
        {
            InteractionDbType = InteractionDbType;
            DbTable = table;
        }
    }
}
