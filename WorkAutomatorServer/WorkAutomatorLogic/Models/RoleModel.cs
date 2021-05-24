using System.Collections.Generic;

using WorkAutomatorLogic.Models.Permission;

namespace WorkAutomatorLogic.Models
{
    public class RoleModel : ModelBase
    {
        public string Name { get; set; }
        public int CompanyId { get; set; }

        public ICollection<PermissionDbModel> DbPermissions { get; set; }
        public ICollection<int> DetectorPermissions { get; set; }
        public ICollection<int> ManufactoryPermissions { get; set; }
        public ICollection<int> PipelineItemPermissions { get; set; }
        public ICollection<int> StorageCellPermissions { get; set; }
    }
}
