using Newtonsoft.Json;
using System;

namespace WorkAutomatorLogic.Models.Event
{
    public class EnterLeavePointEventModel : IdModel
    {
        [JsonProperty("enter_leave_point")]
        public EnterLeavePointModel EnterLeavePoint { get; set; }

        [JsonProperty("timespan")]
        public DateTime Timespan { get; set; }

        [JsonProperty("is_enter")]
        public bool IsEnter { get; set; }

        [JsonProperty("log")]
        public string Log { get; set; }
    }
}
