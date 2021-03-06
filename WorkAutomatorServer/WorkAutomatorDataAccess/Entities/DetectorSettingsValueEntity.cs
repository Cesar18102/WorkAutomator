namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("detector_settings_value")]
    public partial class DetectorSettingsValueEntity : IdEntity
    {
        
        public int detector_id { get; set; }

        public int detector_settings_prefab_id { get; set; }

        [Column(TypeName = "text")]
        public string option_data_value_base64 { get; set; }

        public virtual DetectorEntity detector { get; set; }

        public virtual DetectorSettingsPrefabEntity detector_settings_prefab { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return detector.IsOwnedByCompany(companyId);
        }
    }
}
