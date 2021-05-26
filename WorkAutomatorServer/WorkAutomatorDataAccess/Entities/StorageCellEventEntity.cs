namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("storage_cell_event")]
    public partial class StorageCellEventEntity : IdEntity
    {
        
        public int storage_cell_id { get; set; }

        public int account_id { get; set; }

        public DateTime timespan { get; set; }

        public double amount { get; set; }

        [Column(TypeName = "text")]
        public string log { get; set; }

        public virtual AccountEntity account { get; set; }

        public virtual StorageCellEntity storage_cell { get; set; }
    }
}
