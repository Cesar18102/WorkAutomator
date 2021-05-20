namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PipelineItemSettingsPrefabEntity : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PipelineItemSettingsPrefabEntity()
        {
            pipeline_item_settings_value = new HashSet<PipelineItemSettingsValueEntity>();
        }

        
        public int pipeline_item_prefab_id { get; set; }

        public int option_data_type_id { get; set; }

        [Required]
        [StringLength(256)]
        public string option_name { get; set; }

        [Column(TypeName = "text")]
        public string option_description { get; set; }

        public virtual DataTypeEntity DataType { get; set; }

        public virtual PipelineItemPrefabEntity pipeline_item_prefab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemSettingsValueEntity> pipeline_item_settings_value { get; set; }
    }
}
