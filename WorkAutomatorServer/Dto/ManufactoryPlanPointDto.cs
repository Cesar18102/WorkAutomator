using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto
{
    public class ManufactoryPlanPointDto : IdDto
    {
        [ObjectId(DbTable.ManufactoryPlanPoint)]
        [JsonIgnore]
        public int? ManufactoryPlanPointId => Id;

        [ObjectId(DbTable.CompanyPlanUniquePoint)]
        [JsonProperty("company_plan_point_id")]
        public int? CompanyPlanPointId { get; set; }

        [JsonProperty("fake_company_plan_point_id")]
        public int? FakeCompanyPlanPointId { get; set; }
    }
}
