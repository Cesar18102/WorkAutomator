using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

using Autofac;

using WorkAutomatorServer.Aspects;

using Dto;

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
        [RequiredMaskAspect("dto.Data.Name", "dto.Data.PlanImageUrl")]
        public async Task<HttpResponseMessage> Create([FromBody] AuthorizedDto<CompanyDto> dto)
        {
            return await Execute(company => CompanyService.CreateCompany(company), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.Id", "dto.Data.Name", "dto.Data.PlanImageUrl")]
        public async Task<HttpResponseMessage> Update([FromBody] AuthorizedDto<CompanyDto> dto)
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
        public async Task<HttpResponseMessage> FireMember([FromBody] AuthorizedDto<FireHireDto> dto)
        {
            return await Execute(hireFireDto => CompanyService.FireMember(hireFireDto), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.Id")]
        public async Task<HttpResponseMessage> Get([FromBody] AuthorizedDto<CompanyIdDto> dto)
        {
            return await Execute(companyIdDto => CompanyService.GetCompany(companyIdDto), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> SetupPlan([FromBody] AuthorizedDto<SetupPlanDto> dto)
        {
            return await Execute(plan => CompanyService.SetupPlan(plan), dto);
        }
    }
}
