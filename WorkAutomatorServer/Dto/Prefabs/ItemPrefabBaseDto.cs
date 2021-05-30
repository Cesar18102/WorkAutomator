using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;

namespace Dto.Prefabs
{
    public class ItemPrefabBaseDto : IdDto
    {
        [CompanyId]
        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("input_x")]
        public double? InputX { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("input_y")]
        public double? InputY { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("output_x")]
        public double? OutputX { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("output_y")]
        public double? OutputY { get; set; }
    }
}
