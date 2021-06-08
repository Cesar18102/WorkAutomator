using System.Collections.Generic;
using System.Threading.Tasks;

using Dto;
using Dto.Interaction;
using Dto.Pipeline;

using WorkAutomatorLogic.Models.Pipeline;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IPipelineItemService
    {
        Task<PipelineItemModel> Create(AuthorizedDto<PipelineItemDto> dto);
        Task<ICollection<PipelineItemModel>> Get(AuthorizedDto<CompanyDto> dto);
        Task<PipelineItemModel> SetupSettings(AuthorizedDto<PipelineItemDto> dto);
        Task<PipelineItemModel> SetupDetector(AuthorizedDto<SetupDetectorDto> dto);
        Task UnsetDetector(AuthorizedDto<DetectorDto> dto);
        Task TryInteract(PipelineItemInteractionDto dto);
    }
}
