using System.Collections.Generic;
using System.Threading.Tasks;

using Dto;
using Dto.Pipeline;

using WorkAutomatorLogic.Models.Pipeline;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IStorageCellService
    {
        Task<StorageCellModel> Create(AuthorizedDto<StorageCellDto> dto);
        Task<ICollection<StorageCellModel>> Get(AuthorizedDto<CompanyDto> dto);
    }
}
