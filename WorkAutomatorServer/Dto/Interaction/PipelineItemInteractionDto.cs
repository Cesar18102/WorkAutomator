using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Interaction
{
    public class PipelineItemInteractionDto : DtoBase
    {
        [ObjectId(DbTable.Detector)]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("pipeline_item_id")]
        public int? PipelineItemId { get; set; }

        [InitiatorAccountId]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("account_id")]
        public int? AccountId { get; set; }
    }
}
