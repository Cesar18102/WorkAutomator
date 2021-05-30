using System.Collections.Generic;
using System.Threading.Tasks;

using Dto;
using Dto.Pipeline;

using WorkAutomatorLogic.Models.Pipeline;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IDetectorService
    {
        Task<DetectorModel> Create(AuthorizedDto<DetectorDto> dto);
        Task<ICollection<DetectorModel>> Get(AuthorizedDto<CompanyDto> dto);
        Task<DetectorModel> SetupSettings(AuthorizedDto<DetectorDto> dto);
    }
}
