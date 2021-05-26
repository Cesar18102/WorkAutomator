using Attributes;
using Newtonsoft.Json;

namespace Dto
{
    public class CompanyPlanPointDto : IdDto
    {
        [ObjectId]
        public int? PlanPointId => Id;
        
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }
    }
}
