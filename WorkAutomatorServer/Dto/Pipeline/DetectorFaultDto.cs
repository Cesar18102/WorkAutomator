using Attributes;
using Constants;

using Newtonsoft.Json;

namespace Dto.Pipeline
{
    public class DetectorFaultDto : IdDto
    {
        [ObjectId(DbTable.DetectorFaultPrefab)]
        [JsonIgnore]
        public int? DetectorFaultPrefabId => Id;
    }
}
