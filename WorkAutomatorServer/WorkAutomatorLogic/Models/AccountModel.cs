using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models
{
    public class AccountModel : IdModel
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
    }
}
