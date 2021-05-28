using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto
{
    public class ManufactoryDto : IdDto
    {
        [ObjectId(DbTable.Manufactory)]
        [JsonIgnore]
        public int? ManufactoryId => Id;

        [JsonProperty("manufactory_plan_points")]
        public ManufactoryPlanPointDto[] ManufactoryPlanPoints { get; set; }
    }
}
