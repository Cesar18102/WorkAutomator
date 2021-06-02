using Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WorkAutomatorLogic.Models.Permission
{
    public abstract class PermissionModelBase : IdModel
    {
        [JsonProperty("interaction_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public InteractionType InteractionType { get; private set; }

        public PermissionModelBase(InteractionType interactionType)
        {
            InteractionType = interactionType;
        }
    }
}
