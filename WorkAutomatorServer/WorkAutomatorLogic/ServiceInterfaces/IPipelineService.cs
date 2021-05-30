using System.Threading.Tasks;

using Dto;
using Dto.Pipeline;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IPipelineService
    {
        Task Create(AuthorizedDto<PipelineDto> dto);
        Task Update(AuthorizedDto<PipelineDto> dto);
    }
}
