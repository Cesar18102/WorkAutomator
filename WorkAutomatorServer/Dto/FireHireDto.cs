using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

using Attributes;

namespace Dto
{
    public class FireHireDto : DtoBase
    {
        [SubjectId]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("account_id")]
        public int? AccountId { get; set; }
    }
}
