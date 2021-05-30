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

        //provide data
        //notify fault
    }
}
