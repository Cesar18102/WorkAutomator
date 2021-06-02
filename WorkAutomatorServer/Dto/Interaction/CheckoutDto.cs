using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Interaction
{
    public class CheckoutDto : DtoBase
    {
        [ObjectId(DbTable.CheckPoint)]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("check_point_id")]
        public int? CheckPointId { get; set; }

        [ObjectId(DbTable.Manufactory)]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("current_manufactory_id")]
        public int? CurrentManufactoryId { get; set; }

        [InitiatorAccountId]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("account_id")]
        public int? AccountId { get; set; }
    }
}
