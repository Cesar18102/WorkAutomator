using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto
{
    public class CompanyDto : IdDto
    {
        [CompanyId]
        [ObjectId(DbTable.Company)]
        [JsonIgnore]
        public int? CompanyId => Id;

        [JsonProperty("name")]
        public string Name { get; set; }

        [Url]
        [JsonProperty("plan_image_url")]
        public string PlanImageUrl { get; set; }
    }
}