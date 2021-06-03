using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Autofac;

using Dto;
using Dto.Prefabs;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;
using WorkAutomatorServer.Aspects;

namespace WorkAutomatorServer.Controllers
{
    [EnableCors("*", "*", "*")]
    public class PrefabController : ControllerBase
    {
        private static IPrefabService PrefabService = LogicDependencyHolder.Dependencies.Resolve<IPrefabService>();

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.CompanyId")]
        public async Task<HttpResponseMessage> CreatePipelineItemPrefab([FromBody] AuthorizedDto<PipelineItemPrefabDto> dto)
        {
            return await Execute(prefab => PrefabService.CreatePipelineItemPrefab(prefab), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.CompanyId")]
        public async Task<HttpResponseMessage> CreateStorageCellPrefab([FromBody] AuthorizedDto<StorageCellPrefabDto> dto)
        {
            return await Execute(prefab => PrefabService.CreateStorageCellPrefab(prefab), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.CompanyId")]
        public async Task<HttpResponseMessage> CreateDetectorPrefab([FromBody] AuthorizedDto<DetectorPrefabDto> dto)
        {
            return await Execute(prefab => PrefabService.CreateDetectorPrefab(prefab), dto);
        }
    }
}
