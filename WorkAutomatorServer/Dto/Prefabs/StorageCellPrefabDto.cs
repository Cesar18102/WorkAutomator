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
    }
}
