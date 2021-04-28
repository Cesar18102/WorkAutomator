using Newtonsoft.Json;

namespace WorkAutomatorServer.Output
{
    public class Response
    {
        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("error")]
        public ErrorPart Error { get; set; }
    }
}