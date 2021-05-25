using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

namespace WorkAutomatorServer.Dto
{
    public class CompanyDto : IdDto
    {
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Url]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("plan_image_url")]
        public string PlanImageUrl { get; set; }
    }
}