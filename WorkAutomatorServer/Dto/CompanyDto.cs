using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;

namespace Dto
{
    public class CompanyDto : IdDto
    {
        [SubjectId]
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