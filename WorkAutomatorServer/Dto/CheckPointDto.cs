using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto
{
    public class CheckPointDto : IdDto
    {
        [ObjectId(DbTable.CompanyPlanUniquePoint)]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("company_plan_unique_point1_id")]
        public int? CompanyPlanUniquePoint1Id { get; set; }

        [ObjectId(DbTable.CompanyPlanUniquePoint)]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("company_plan_unique_point2_id")]
        public int? CompanyPlanUniquePoint2Id { get; set; }
    }
}
