using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Interaction
{
    public class DetectorInteractionDto : DtoBase
    {
        [ObjectId(DbTable.Detector)]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("detector_id")]
        public int? DetectorId { get; set; }

        [InitiatorAccountId]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("account_id")]
        public int? AccountId { get; set; }
    }
}
