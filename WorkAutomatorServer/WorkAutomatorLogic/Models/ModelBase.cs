using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models
{
    public abstract class ModelBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
