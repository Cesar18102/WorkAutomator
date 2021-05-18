namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PipelineItemPrefabEntity : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PipelineItemPrefabEntity()
        {
            pipeline_item = new HashSet<PipelineItemEntity>();
            pipeline_item_settings_prefab = new HashSet<PipelineItemSettingsPrefabEntity>();
        }

        
        public int company_id { get; set; }

        [Required]
        [StringLength(1024)]
        public string name { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }

        [Required]
        [StringLength(1024)]
        public string image_url { get; set; }

        public double input_x { get; set; }

        public double input_y { get; set; }

        public double output_x { get; set; }

        public double output_y { get; set; }

        public virtual CompanyEntity company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemEntity> pipeline_item { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemSettingsPrefabEntity> pipeline_item_settings_prefab { get; set; }
    }
}
