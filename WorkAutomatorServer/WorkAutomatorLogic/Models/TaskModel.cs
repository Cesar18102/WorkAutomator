using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models
{
    public class TaskModel : IdModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("company_id")]
        public int CompanyId { get; set; }

        [JsonProperty("is_done")]
        public bool IsDone { get; set; }

        [JsonProperty("is_reviewed")]
        public bool IsReviewed { get; set; }

        [JsonProperty("associated_fault_id")]
        public int? AssociatedFaultId { get; set; }

        [JsonProperty("creator")]
        public AccountModel Creator { get; set; }

        [JsonProperty("assignee")]
        public AccountModel Assignee { get; set; }

        [JsonProperty("reviewer")]
        public AccountModel Reviewer { get; set; }
    }
}
