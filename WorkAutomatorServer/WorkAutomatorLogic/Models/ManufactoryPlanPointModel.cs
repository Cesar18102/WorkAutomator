using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models
{
    public class ManufactoryPlanPointModel : IdModel
    {
        [JsonProperty("manufactory_id")]
        public int ManufactoryId { get; set; }

        [JsonProperty("company_plan_point_id")]
        public int CompanyPlanPointId { get; set; }
    }
}
