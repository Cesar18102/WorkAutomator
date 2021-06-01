using System.Threading.Tasks;

using Dto;
using Dto.Pipeline;

using WorkAutomatorLogic.Models.Pipeline;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IPipelineService
    {
        Task<PipelineModel> Create(AuthorizedDto<PipelineDto> dto);
        Task<PipelineModel> Update(AuthorizedDto<PipelineDto> dto);
    }
}
