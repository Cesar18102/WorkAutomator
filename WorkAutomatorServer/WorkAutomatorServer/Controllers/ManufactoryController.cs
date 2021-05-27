using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

using Autofac;

using Dto;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;

using WorkAutomatorServer.Aspects;

namespace WorkAutomatorServer.Controllers
{
    public class ManufactoryController : ControllerBase
    {
        private static IManufactoryService ManufactoryService = LogicDependencyHolder.Dependencies.Resolve<IManufactoryService>();

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.CompanyId", "dto.Data.ManufactoryPlanPoints.Id")]
        public async Task<HttpResponseMessage> Create([FromBody] AuthorizedDto<ManufactoryDto> dto)
        {
            return await Execute(manufactory => ManufactoryService.CreateManufactory(manufactory), dto);
        }
    }
}
