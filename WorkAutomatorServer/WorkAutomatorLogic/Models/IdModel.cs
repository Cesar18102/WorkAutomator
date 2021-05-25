using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models
{
    public class IdModel : ModelBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
