namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pipeline_item_storage_connection")]
    public partial class PipelineItemStorageConnectionEntity : IdEntity
    {
        
        public int pipeline_item_id { get; set; }

        public int storage_cell_id { get; set; }

        public bool is_direct { get; set; }

        public virtual PipelineItemEntity pipeline_item { get; set; }

        public virtual StorageCellEntity StorageCell { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return pipeline_item.IsOwnedByCompany(companyId);
        }
    }
}
