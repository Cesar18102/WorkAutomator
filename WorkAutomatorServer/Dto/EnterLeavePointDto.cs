using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto
{
    public class EnterLeavePointDto : IdDto
    {
        [ObjectId(DbTable.EnterLeavePoint)]
        [JsonIgnore]
        public int? EnterLeavePointId => Id;

        [ObjectId(DbTable.CompanyPlanUniquePoint)]
        [JsonProperty("company_plan_unique_point1_id")]
        public int? CompanyPlanUniquePoint1Id { get; set; }

        [ObjectId(DbTable.CompanyPlanUniquePoint)]
        [JsonProperty("company_plan_unique_point2_id")]
        public int? CompanyPlanUniquePoint2Id { get; set; }

        [JsonProperty("fake_company_plan_unique_point1_id")]
        public int? FakeCompanyPlanUniquePoint1Id { get; set; }

        [JsonProperty("fake_company_plan_unique_point2_id")]
        public int? FakeCompanyPlanUniquePoint2Id { get; set; }
    }
}
