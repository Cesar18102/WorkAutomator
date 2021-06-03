using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Autofac;

using Dto;
using Dto.DetectorData;
using Dto.Interaction;
using Dto.Pipeline;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;

using WorkAutomatorServer.Aspects;

namespace WorkAutomatorServer.Controllers
{
    [EnableCors("*", "*", "*")]
    public class DetectorController : ControllerBase
    {
        private static IDetectorService DetectorService = LogicDependencyHolder.Dependencies.Resolve<IDetectorService>();

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.PrefabId")]
        public async Task<HttpResponseMessage> Create([FromBody] AuthorizedDto<DetectorDto> dto)
        {
            return await Execute(detector => DetectorService.Create(detector), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.Id")]
        public async Task<HttpResponseMessage> SetupSettings([FromBody] AuthorizedDto<DetectorDto> dto)
        {
            return await Execute(detector => DetectorService.SetupSettings(detector), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.Id")]
        public async Task<HttpResponseMessage> Get([FromBody] AuthorizedDto<CompanyDto> dto)
        {
            return await Execute(d => DetectorService.Get(d), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [RequiredMaskAspect("dto.Id", "dto.Data.DetectorDataPrefabId")]
        public async Task<HttpResponseMessage> ProvideData([FromBody] DetectorDataDto dto)
        {
            return await Execute(data => DetectorService.ProvideData(data), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.Id")]
        public async Task<HttpResponseMessage> GetData([FromBody] AuthorizedDto<GetDetectorDataDto> dto)
        {
            return await Execute(d => DetectorService.GetData(d), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.Id")]
        public async Task<HttpResponseMessage> GetActualFaults([FromBody] AuthorizedDto<DetectorDto> dto)
        {
            return await Execute(d => DetectorService.GetActualFaults(d), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.Id")]
        public async Task<HttpResponseMessage> GetAllFaults([FromBody] AuthorizedDto<GetDetectorDataDto> dto)
        {
            return await Execute(d => DetectorService.GetAllFaults(d), dto);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> TryInteract([FromBody] DetectorInteractionDto dto)
        {
            return await Execute(d => DetectorService.TryInteract(d), dto);
        }
    }
}
