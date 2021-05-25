using Newtonsoft.Json;

namespace Dto
{
    public class IdDto : DtoBase
    {
        [JsonProperty("id")]
        public int? Id { get; set; }
    }
}
