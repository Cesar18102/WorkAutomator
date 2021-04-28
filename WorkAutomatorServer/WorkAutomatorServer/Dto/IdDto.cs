using Newtonsoft.Json;

namespace WorkAutomatorServer.Dto
{
    public class IdDto : DtoBase
    {
        [JsonProperty("id")]
        public int? Id { get; set; }
    }
}
