using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models
{
    public class CheckPointModel : IdModel
    {
        [JsonProperty("company_plan_unique_point1_id")]
        public int CompanyPlanUniquePoint1Id { get; set; }

        [JsonProperty("company_plan_unique_point2_id")]
        public int CompanyPlanUniquePoint2Id { get; set; }

        [JsonProperty("manufactory1_id")]
        public int Manufactory1Id { get; set; }

        [JsonProperty("manufactory2_id")]
        public int Manufactory2Id { get; set; }
    }
}
