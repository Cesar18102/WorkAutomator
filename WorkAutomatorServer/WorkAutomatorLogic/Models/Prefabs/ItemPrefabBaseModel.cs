using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models.Prefabs
{
    public class ItemPrefabBaseModel : IdModel
    {
        [JsonProperty("company_id")]
        public int CompanyId { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("input_x")]
        public double InputX { get; set; }

        [JsonProperty("input_y")]
        public double InputY { get; set; }

        [JsonProperty("output_x")]
        public double OutputX { get; set; }

        [JsonProperty("output_y")]
        public double OutputY { get; set; }
    }
}
