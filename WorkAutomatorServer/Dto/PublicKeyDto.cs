using Newtonsoft.Json;

namespace Dto
{
    public class PublicKeyDto : DtoBase
    {
        [JsonProperty("modulus")]
        public string Modulus { get; set; }

        [JsonProperty("exponent")]
        public string Exponent { get; set; }
    }
}