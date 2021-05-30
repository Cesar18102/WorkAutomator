using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Autofac;

using Dto;
using Dto.Pipeline;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;

using WorkAutomatorServer.Aspects;

namespace WorkAutomatorServer.Controllers
{
    public class PipelineController : ControllerBase
    {
        private static IPipelineService PipelineService = LogicDependencyHolder.Dependencies.Resolve<IPipelineService>();

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect(
            "dto.Data.CompanyId", "dto.Data.Connections.InputPipelineItems.Id", "dto.Data.Connections.OutputPipelineItems.Id",
            "dto.Data.Connections.InputStorageCells.Id", "dto.Data.Connections.OutputStorageCells.Id"
        )]
        public async Task<HttpResponseMessage> Create([FromBody] AuthorizedDto<PipelineDto> dto)
        {
            return await Execute(pipeline => PipelineService.Create(pipeline), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect(
            "dto.Data.Id", "dto.Data.Connections.InputPipelineItems.Id", "dto.Data.Connections.OutputPipelineItems.Id",
            "dto.Data.Connections.InputStorageCells.Id", "dto.Data.Connections.OutputStorageCells.Id"
        )]
        public async Task<HttpResponseMessage> Update([FromBody] AuthorizedDto<PipelineDto> dto)
        {
            return await Execute(pipeline => PipelineService.Create(pipeline), dto);
        }
    }
}
