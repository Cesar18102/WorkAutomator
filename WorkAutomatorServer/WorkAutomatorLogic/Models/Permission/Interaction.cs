namespace WorkAutomatorLogic.Models.Permission
{
    public class Interaction
    {
        public PermissionModelBase Permission { get; set; }
        public int InitiatorAccountId { get; set; }

        public Interaction(PermissionModelBase permission, int initiatorAccountId)
        {
            Permission = permission;
            InitiatorAccountId = initiatorAccountId;
        }
    }
}
