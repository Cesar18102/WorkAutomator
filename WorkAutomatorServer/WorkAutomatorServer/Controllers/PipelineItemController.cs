using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Autofac;

using Dto;
using Dto.Interaction;
using Dto.Pipeline;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;
using WorkAutomatorServer.Aspects;

namespace WorkAutomatorServer.Controllers
{
    [EnableCors("*", "*", "*")]
    public class PipelineItemController : ControllerBase
    {
        private static IPipelineItemService PipelineItemService = LogicDependencyHolder.Dependencies.Resolve<IPipelineItemService>();

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.PrefabId")]
        public async Task<HttpResponseMessage> Create([FromBody] AuthorizedDto<PipelineItemDto> dto)
        {
            return await Execute(pipelineItem => PipelineItemService.Create(pipelineItem), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.Id")]
        public async Task<HttpResponseMessage> SetupSettings([FromBody] AuthorizedDto<PipelineItemDto> dto)
        {
            return await Execute(pipelineItem => PipelineItemService.SetupSettings(pipelineItem), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> SetupDetector([FromBody] AuthorizedDto<SetupDetectorDto> dto)
        {
            return await Execute(setupDetectorDto => PipelineItemService.SetupDetector(setupDetectorDto), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> Get([FromBody] AuthorizedDto<CompanyDto> dto)
        {
            return await Execute(d => PipelineItemService.Get(d), dto);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> TryInteract([FromBody] PipelineItemInteractionDto dto)
        {
            return await Execute(d => PipelineItemService.TryInteract(d), dto);
        }
    }
}
