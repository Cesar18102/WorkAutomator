using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Autofac;

using Dto;
using Dto.Pipeline;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;

using WorkAutomatorServer.Aspects;

namespace WorkAutomatorServer.Controllers
{
    [EnableCors("*", "*", "*")]
    public class PipelineController : ControllerBase
    {
        private static IPipelineService PipelineService = LogicDependencyHolder.Dependencies.Resolve<IPipelineService>();

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect(
            "dto.Data.CompanyId", "dto.Data.Connections.InputPipelineItems.Id", "dto.Data.Connections.OutputPipelineItems.Id",
            "dto.Data.Connections.InputStorageCells.Id", "dto.Data.Connections.OutputStorageCells.Id",
            "dto.Data.PipelineItemPlacements.Id", "dto.Data.StorageCellPlacements.Id"
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
            "dto.Data.Connections.InputStorageCells.Id", "dto.Data.Connections.OutputStorageCells.Id",
            "dto.Data.PipelineItemPlacements.Id", "dto.Data.StorageCellPlacements.Id"
        )]
        public async Task<HttpResponseMessage> Update([FromBody] AuthorizedDto<PipelineDto> dto)
        {
            return await Execute(pipeline => PipelineService.Update(pipeline), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.Id")]
        public async Task<HttpResponseMessage> Scrap([FromBody] AuthorizedDto<PipelineDto> dto)
        {
            return await Execute(pipeline => PipelineService.Scrap(pipeline), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.Id")]
        public async Task<HttpResponseMessage> Get([FromBody] AuthorizedDto<CompanyDto> dto)
        {
            return await Execute(company => PipelineService.Get(company), dto);
        }
    }
}
