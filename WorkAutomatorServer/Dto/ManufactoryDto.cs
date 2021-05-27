using Newtonsoft.Json;

using Attributes;

namespace Dto
{
    public class ManufactoryDto : IdDto
    {
        [CompanyId]
        [JsonProperty("company_id")]
        public int CompanyId { get; set; }

        [JsonProperty("manufactory_plan_points")]
        public ManufactoryPlanPointDto[] ManufactoryPlanPoints { get; set; }
    }
}
