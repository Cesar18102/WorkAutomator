using System.Threading.Tasks;

using Dto;
using Dto.Pipeline;

using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class PipelineService : IPipelineService
    {
        public Task Create(AuthorizedDto<PipelineDto> dto)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(AuthorizedDto<PipelineDto> dto)
        {
            throw new System.NotImplementedException();
        }
    }
}
