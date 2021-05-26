using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

using Autofac;

using WorkAutomatorServer.Aspects;

using Dto;
using Attributes;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorServer.Controllers
{
    public class CompanyController : ControllerBase
    {
        private static ICompanyService CompanyService = LogicDependencyHolder.Dependencies.Resolve<ICompanyService>();

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> Create([FromBody] AuthorizedDto<CompanyDto> dto)
        {
            return await Execute(company => CompanyService.CreateCompany(company), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> Update([FromBody, Identified] AuthorizedDto<CompanyDto> dto)
        {
            return await Execute(company => CompanyService.UpdateCompany(company), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> HireMember([FromBody] AuthorizedDto<FireHireDto> dto)
        {
            return await Execute(hireFireDto => CompanyService.HireMember(hireFireDto), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> FireMember([FromBody, Identified] AuthorizedDto<FireHireDto> dto)
        {
            return await Execute(hireFireDto => CompanyService.FireMember(hireFireDto), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> SetupPlanPoints([FromBody, Identified] AuthorizedDto<CompanyPlanPointsDto> dto)
        {
            return await Execute(planPointsDto => CompanyService.SetupCompanyPlanPoints(planPointsDto), dto);
        }
    }
}
