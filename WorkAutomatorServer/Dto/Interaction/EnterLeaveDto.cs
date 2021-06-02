using Attributes;
using Constants;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Dto.Interaction
{
    public class EnterLeaveDto : DtoBase
    {
        [ObjectId(DbTable.EnterLeavePoint)]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("enter_leave_point_id")]
        public int? EnterLeavePointId { get; set; }

        [InitiatorAccountId]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("account_id")]
        public int? AccountId { get; set; }
    }
}
