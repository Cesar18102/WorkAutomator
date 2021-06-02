using Constants;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WorkAutomatorLogic.Models.Permission
{
    public class PermissionDbModel : PermissionModelBase
    {
        [JsonProperty("db_interaction_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public InteractionDbType InteractionDbType { get; set; }

        [JsonProperty("table")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DbTable DbTable { get; set; }

        public PermissionDbModel(InteractionDbType interactionDbType, DbTable table) : base(InteractionType.DB)
        {
            InteractionDbType = interactionDbType;
            DbTable = table;
        }

        public PermissionDbModel() : base(InteractionType.DB) { }
    }
}
