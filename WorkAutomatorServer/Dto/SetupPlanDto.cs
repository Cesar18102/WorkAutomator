using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;

namespace Dto
{
    public class SetupPlanDto : DtoBase
    {
        [CompanyId]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }

        [JsonProperty("company_plan_points")]
        public CompanyPlanPointDto[] CompanyPlanPoints { get; set; }

        [JsonProperty("manufactories")]
        public ManufactoryDto[] Manufactories { get; set; }

        [JsonProperty("check_points")]
        public CheckPointDto[] CheckPoints { get; set; }

        [JsonProperty("enter_leave_points")]
        public EnterLeavePointDto[] EnterLeavePoints { get; set; }
    }
}
