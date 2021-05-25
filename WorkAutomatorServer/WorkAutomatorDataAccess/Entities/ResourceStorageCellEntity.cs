namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ResourceStorageCellEntity : IdEntity
    {
        
        public int resource_id { get; set; }

        public int storage_cell_id { get; set; }

        public double amount { get; set; }

        public virtual ResourceEntity Resource { get; set; }

        public virtual StorageCellEntity storage_cell { get; set; }
    }
}
