using System.ComponentModel.DataAnnotations;

using Attributes;
using Constants;

using Newtonsoft.Json;

namespace Dto.Tasks
{
    public class ReviewTaskDto : IdDto
    {
        [ObjectId(DbTable.Task)]
        [JsonIgnore]
        public int? TaskId => Id;

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("review_result")]
        public bool? ReviewResult { get; set; }
    }
}
