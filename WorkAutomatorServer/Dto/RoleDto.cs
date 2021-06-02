using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Attributes;
using Constants;

using Newtonsoft.Json;

namespace Dto
{
    public class RoleDto : IdDto
    {
        [ObjectId(DbTable.Role)]
        [JsonIgnore]
        public int? RoleId => Id;

        [CompanyId]
        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [ObjectId(DbTable.DbPermission)]
        [JsonProperty("db_permission_ids")]
        public ICollection<int> DbPermissionIds { get; set; }

        [Required(AllowEmptyStrings = false)]
        [ObjectId(DbTable.Manufactory)]
        [JsonProperty("manufactory_ids")]
        public ICollection<int> ManufactoryIds { get; set; }

        [ObjectId(DbTable.StorageCell)]
        [JsonProperty("storage_cell_ids")]
        public ICollection<int> StorageCellIds { get; set; }

        [Required(AllowEmptyStrings = false)]
        [ObjectId(DbTable.PipelineItem)]
        [JsonProperty("pipeline_item_ids")]
        public ICollection<int> PipelineItemIds { get; set; }

        [Required(AllowEmptyStrings = false)]
        [ObjectId(DbTable.Detector)]
        [JsonProperty("detector_ids")]
        public ICollection<int> DetectorIds { get; set; }
    }
}
