using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Interaction
{
    public class StorageCellInteractionDto : DtoBase
    {
        [ObjectId(DbTable.StorageCell)]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("storage_cell_id")]
        public int? StorageCellId { get; set; }

        [InitiatorAccountId]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("account_id")]
        public int? AccountId { get; set; }
    }
}
