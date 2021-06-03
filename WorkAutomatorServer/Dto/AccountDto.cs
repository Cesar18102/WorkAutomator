using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace Dto
{
    public class AccountDto : IdDto
    {
        [RegularExpression("[A-Za-z]{1,32}", ErrorMessage = "first_name must consist of letters between 1 and 32 symbols")]
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [RegularExpression("[A-Za-z]{1,32}", ErrorMessage = "last_name must consist of letters between 1 and 32 symbols")]
        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }
}
