using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models
{
    public class PlanPointModel : IdModel
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }
    }
}
