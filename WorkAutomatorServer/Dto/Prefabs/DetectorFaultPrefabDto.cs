using Attributes;
using Constants;

using Newtonsoft.Json;

namespace Dto.Prefabs
{
    public class DetectorFaultPrefabDto : IdDto
    {
        [ObjectId(DbTable.DetectorFaultPrefab)]
        [JsonIgnore]
        public int? DetectorFaultPrefabId => Id;

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fault_condition")]
        public string FaultCondition { get; set; }
    }
}
