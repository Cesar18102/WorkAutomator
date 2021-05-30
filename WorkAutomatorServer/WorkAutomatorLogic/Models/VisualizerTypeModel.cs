using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models
{
    public class VisualizerTypeModel : IdModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
