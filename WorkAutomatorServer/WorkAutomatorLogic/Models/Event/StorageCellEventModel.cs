using Newtonsoft.Json;
using System;
using WorkAutomatorLogic.Models.Pipeline;

namespace WorkAutomatorLogic.Models.Event
{
    public class StorageCellEventModel : IdModel
    {
        [JsonProperty("storage_cell")]
        public StorageCellModel StorageCell { get; set; }

        [JsonProperty("timespan")]
        public DateTime Timespan { get; set; }

        [JsonProperty("log")]
        public string Log { get; set; }

        [JsonProperty("amount")]
        public double amount { get; set; }
    }
}
