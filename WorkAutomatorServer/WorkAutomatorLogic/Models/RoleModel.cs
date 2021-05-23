using System.Collections.Generic;

namespace WorkAutomatorLogic.Models
{
    public class RoleModel : ModelBase
    {
        public string Name { get; set; }
        public int CompanyId { get; set; }

        public ICollection<DbPermissionModel> DbPermissions { get; set; }
        public ICollection<int> DetectorPermissions { get; set; }
        public ICollection<int> ManufactoryPermissions { get; set; }
        public ICollection<int> PipelineItemPermissions { get; set; }
        public ICollection<int> StorageCellPermissions { get; set; }
    }
}
