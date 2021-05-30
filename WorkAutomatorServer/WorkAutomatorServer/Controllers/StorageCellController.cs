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
    public class StorageCellController : ControllerBase
    {
        private static IStorageCellService StorageCellService = LogicDependencyHolder.Dependencies.Resolve<IStorageCellService>();

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.PrefabId")]
        public async Task<HttpResponseMessage> Create([FromBody] AuthorizedDto<StorageCellDto> dto)
        {
            return await Execute(pipelineItem => StorageCellService.Create(pipelineItem), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> Get([FromBody] AuthorizedDto<CompanyDto> dto)
        {
            return await Execute(d => StorageCellService.Get(d), dto);
        }
    }
}
