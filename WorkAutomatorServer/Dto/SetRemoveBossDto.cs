using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto
{
    public class SetRemoveBossDto : DtoBase
    {
        [Required(AllowEmptyStrings = false)]
        [ObjectId(DbTable.Account)]
        [JsonProperty("sub_account_id")]
        public int? SubAccountId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [ObjectId(DbTable.Account)]
        [JsonProperty("boss_account_id")]
        public int? BossAccountId { get; set; }
    }
}
