using System.Collections.Generic;

using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models.Pipeline
{
    public class PipelineModel : IdModel
    {
        [JsonProperty("company_id")]
        public int CompanyId { get; set; }

        [JsonProperty("connections")]
        public ICollection<PipelineItemConnectionModel> Connections { get; set; }
    }
}
