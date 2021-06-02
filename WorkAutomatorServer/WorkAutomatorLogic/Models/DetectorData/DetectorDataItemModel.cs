using System;

using Newtonsoft.Json;

using WorkAutomatorLogic.Models.Prefabs;

namespace WorkAutomatorLogic.Models.DetectorData
{
    public class DetectorDataItemModel 
    {
        [JsonProperty("data_prefab")]
        public DetectorDataPrefabModel DataPrefab { get; set; }

        [JsonProperty("data_base64")]
        public string DataBase64 { get; set; }

        [JsonProperty("timespan")]
        public DateTime Timespan { get; set; }
    }
}
