using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Prefabs
{
    public class StorageCellPrefabDto : ItemPrefabBaseDto
    {
        [ObjectId(DbTable.StorageCellPrefab)]
        [JsonIgnore]
        public int? StorageCellPrefabId => Id;

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
