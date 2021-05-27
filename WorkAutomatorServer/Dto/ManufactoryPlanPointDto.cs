using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto
{
    public class ManufactoryPlanPointDto : IdDto
    {
        [ObjectId(DbTable.ManufactoryPlanPoint)]
        [JsonIgnore]
        public int? PointId => Id;
    }
}
