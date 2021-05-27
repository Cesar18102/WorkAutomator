using Attributes;
using Constants;

namespace Dto
{
    public class CompanyIdDto : IdDto
    {
        [CompanyId]
        [ObjectId(DbTable.Company)]
        public int? CompanyId => Id;
    }
}
