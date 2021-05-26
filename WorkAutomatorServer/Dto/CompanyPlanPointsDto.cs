using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;

namespace Dto
{
    public class CompanyPlanPointsDto : IdDto
    {
        [CompanyId]
        [JsonIgnore]
        public int? CompanyId => Id;

        [Required]
        [JsonProperty("points")]
        public CompanyPlanPointDto[] Points { get; set; }
    }
}
