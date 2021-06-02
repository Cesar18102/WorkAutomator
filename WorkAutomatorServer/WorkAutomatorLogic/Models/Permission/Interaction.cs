using Constants;

namespace WorkAutomatorLogic.Models.Permission
{
    public class Interaction
    {
        public InteractionType InteractionType { get; set; }
        public InteractionDbType InteractionDbType { get; set; }
        public DbTable Table { get; set; }
        public int InitiatorAccountId { get; set; }
        public int? CompanyId { get; set; }
        public int[] ObjectIds { get; set; }
        public bool CheckSameCompany { get; set; }

        public Interaction(InteractionDbType type, DbTable table, int initiatorAccountId)
        {
            InteractionType = InteractionType.DB;
            InteractionDbType = type;
            Table = table;
            InitiatorAccountId = initiatorAccountId;
        }

        public Interaction(InteractionType type, int initiatorAccountId)
        {
            InteractionType = type;
            InitiatorAccountId = initiatorAccountId;
        }
    }
}
