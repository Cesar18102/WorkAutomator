using System.ComponentModel.DataAnnotations.Schema;

namespace WorkAutomatorDataAccess.Entities
{
    public abstract class EntityBase
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
