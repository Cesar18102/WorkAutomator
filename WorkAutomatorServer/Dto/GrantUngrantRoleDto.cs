using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto
{
    public class GrantUngrantRoleDto : DtoBase
    {
        [Required(AllowEmptyStrings = false)]
        [ObjectId(DbTable.Role)]
        [JsonProperty("role_id")]
        public int? RoleId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [ObjectId(DbTable.Account)]
        [JsonProperty("grant_to_account_id")]
        public int? GrantToAccountId { get; set; }
    }
}
