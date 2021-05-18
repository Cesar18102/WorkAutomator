namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DetectorDataEntity : EntityBase
    {
        
        public int detector_id { get; set; }

        public int detector_data_prefab_id { get; set; }

        [Column(TypeName = "text")]
        public string field_data_value_base64 { get; set; }

        public DateTime timespan { get; set; }

        public virtual DetectorEntity detector { get; set; }

        public virtual DetectorDataPrefabEntity detector_data_prefab { get; set; }
    }
}
