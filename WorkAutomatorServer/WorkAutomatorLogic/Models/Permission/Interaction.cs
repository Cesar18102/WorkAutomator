using System.Collections.Generic;

using Constants;

namespace WorkAutomatorLogic.Models.Permission
{
    public class Interaction
    {
        public PermissionModelBase Permission { get; set; }
        public int InitiatorAccountId { get; set; }
        public int? CompanyId { get; set; }
        public int[] ObjectIds { get; set; }

        public Interaction(InteractionDbType type, DbTable table, int initiatorAccountId)
        {
            Permission = new PermissionDbModel(type, table);
            InitiatorAccountId = initiatorAccountId;
        }

        public Interaction(InteractionType type, int targetId, int initiatorAccountId)
        {
            Permission = new PermissionModel(type, targetId);
            InitiatorAccountId = initiatorAccountId;
        }

        public Interaction(PermissionModelBase permission, int initiatorAccountId)
        {
            Permission = permission;
            InitiatorAccountId = initiatorAccountId;
        }
    }
}
