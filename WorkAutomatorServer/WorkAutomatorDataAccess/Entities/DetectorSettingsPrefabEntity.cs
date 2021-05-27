namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("detector_settings_prefab")]
    public partial class DetectorSettingsPrefabEntity : IdEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DetectorSettingsPrefabEntity()
        {
            detector_settings_value = new HashSet<DetectorSettingsValueEntity>();
        }

        
        public int detector_prefab_id { get; set; }

        public int option_data_type_id { get; set; }

        [Required]
        [StringLength(256)]
        public string option_name { get; set; }

        [Column(TypeName = "text")]
        public string option_description { get; set; }

        public virtual DataTypeEntity DataType { get; set; }

        public virtual DetectorPrefabEntity detector_prefab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorSettingsValueEntity> detector_settings_value { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return detector_prefab.IsOwnedByCompany(companyId);
        }
    }
}
