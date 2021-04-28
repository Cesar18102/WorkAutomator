using Newtonsoft.Json;

namespace WorkAutomatorServer.Dto
{
    public class PublicKeyDto : DtoBase
    {
        [JsonProperty("modulus")]
        public string Modulus { get; set; }

        [JsonProperty("exponent")]
        public string Exponent { get; set; }
    }
}