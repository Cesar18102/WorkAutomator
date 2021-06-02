using System.Collections.Generic;
using System.Threading.Tasks;

using Dto;
using Dto.DetectorData;
using Dto.Pipeline;
using Dto.Interaction;

using WorkAutomatorLogic.Models.DetectorData;
using WorkAutomatorLogic.Models.Pipeline;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IDetectorService
    {
        Task<DetectorModel> Create(AuthorizedDto<DetectorDto> dto);
        Task<ICollection<DetectorModel>> Get(AuthorizedDto<CompanyDto> dto);
        Task<DetectorModel> SetupSettings(AuthorizedDto<DetectorDto> dto);

        Task ProvideData(DetectorDataDto dto);
        Task<DetectorDataModel> GetData(AuthorizedDto<GetDetectorDataDto> dto);

        Task<ICollection<DetectorFaultEventModel>> GetActualFaults(AuthorizedDto<DetectorDto> dto);
        Task<ICollection<DetectorFaultEventModel>> GetAllFaults(AuthorizedDto<GetDetectorDataDto> dto);

        Task TryInteract(DetectorInteractionDto dto);
    }
}
