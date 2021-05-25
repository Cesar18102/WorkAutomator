using Newtonsoft.Json;

using System;

namespace WorkAutomatorLogic.Models
{
    public class SessionModel : ModelBase
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("session_token")]
        public string Token { get; set; }

        [JsonProperty("expired_at")]
        public DateTime ExpiredAt { get; set; }
    }
}
