using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto
{
    public class CompanyPlanPointDto : IdDto
    {
        [ObjectId(DbTable.CompanyPlanUniquePoint)]
        [JsonIgnore]
        public int? PlanPointId => Id;
        
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }
    }
}
