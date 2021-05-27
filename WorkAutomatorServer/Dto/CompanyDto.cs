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

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Url]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("plan_image_url")]
        public string PlanImageUrl { get; set; }
    }
}