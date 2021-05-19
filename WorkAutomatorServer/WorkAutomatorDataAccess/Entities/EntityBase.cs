using System.ComponentModel.DataAnnotations.Schema;

namespace WorkAutomatorDataAccess.Entities
{
    public abstract class EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
    }
}
