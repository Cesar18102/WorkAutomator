using System.Threading.Tasks;

using Dto;
using Dto.Pipeline;

using WorkAutomatorLogic.Models.Pipeline;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IPipelineItemService
    {
        Task<PipelineItemModel> Create(AuthorizedDto<PipelineItemDto> dto);
        Task<PipelineItemModel> SetupSettings(AuthorizedDto<PipelineItemDto> dto);
        //setup detector
    }
}
