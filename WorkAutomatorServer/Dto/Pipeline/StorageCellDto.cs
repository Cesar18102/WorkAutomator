using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Pipeline
{
    public class StorageCellDto : IdDto
    {
        [ObjectId(DbTable.StorageCell)]
        [JsonIgnore]
        public int? StorageCellId => Id;

        [ObjectId(DbTable.StorageCellPrefab)]
        [JsonProperty("prefab_id")]
        public int? PrefabId { get; set; }
    }
}
