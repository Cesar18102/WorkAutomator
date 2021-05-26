namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("detector_fault_event")]
    public partial class DetectorFaultEventEntity : IdEntity
    {
        
        public int detector_fault_id { get; set; }

        public DateTime timespan { get; set; }

        [Column(TypeName = "text")]
        public string log { get; set; }

        public virtual DetectorFaultEntity detector_fault { get; set; }
    }
}
