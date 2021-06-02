using Newtonsoft.Json;
using System.Collections.Generic;

using WorkAutomatorLogic.Models.Permission;

namespace WorkAutomatorLogic.Models.Roles
{
    public class RoleModel : IdModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("company_id")]
        public int CompanyId { get; set; }

        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }

        [JsonProperty("db_permissions")]
        public ICollection<PermissionDbModel> DbPermissions { get; set; }

        [JsonProperty("detector_permissions")]
        public ICollection<int> DetectorPermissions { get; set; }

        [JsonProperty("manufactory_permissions")]
        public ICollection<int> ManufactoryPermissions { get; set; }

        [JsonProperty("pipeline_item_permissions")]
        public ICollection<int> PipelineItemPermissions { get; set; }

        [JsonProperty("storage_cell_permissions")]
        public ICollection<int> StorageCellPermissions { get; set; }
    }
}
