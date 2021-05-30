using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models
{
    public class DataTypeModel : IdModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
