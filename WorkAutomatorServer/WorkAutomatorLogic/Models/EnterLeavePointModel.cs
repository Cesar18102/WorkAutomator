using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models
{
    public class EnterLeavePointModel : IdModel
    {
        [JsonProperty("manufactory_id")]
        public int ManufactoryId { get; set; }

        [JsonProperty("company_plan_unique_point1_id")]
        public int CompanyPlanUniquePoint1Id { get; set; }

        [JsonProperty("company_plan_unique_point2_id")]
        public int CompanyPlanUniquePoint2Id { get; set; }
    }
}
