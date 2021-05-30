using Autofac;
using Dto;
using Dto.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;
using WorkAutomatorServer.Aspects;

namespace WorkAutomatorServer.Controllers
{
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
    }
}
