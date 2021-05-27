using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto
{
    public class ManufactoryPlanPointDto : IdDto
    {
        [ObjectId(DbTable.CompanyPlanUniquePoint)]
        [JsonIgnore]
        public int? PointId => Id;
    }
}
