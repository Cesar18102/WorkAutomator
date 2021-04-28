using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using WorkAutomatorServer.Dto.Attributes;

namespace WorkAutomatorServer.Dto
{
    public class AuthorizedDto<TDto> : DtoBase where TDto : DtoBase
    {
        [HeaderAutoWired]
        public SessionDto Session { get; set; }
        
        [Required(ErrorMessage = "data is required")]
        [JsonProperty("data")]
        public TDto Data { get; set; }
    }
}
