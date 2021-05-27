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
        [RequiredMaskAspect("dto.Data.Id")]
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
        public async Task<HttpResponseMessage> SetupPlanPoints([FromBody] AuthorizedDto<CompanyPlanPointsDto> dto)
        {
            return await Execute(planPointsDto => CompanyService.SetupCompanyPlanPoints(planPointsDto), dto);
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
        public async Task<HttpResponseMessage> SetupCheckPoint([FromBody] AuthorizedDto<CheckPointDto> dto)
        {
            return await Execute(checkPointDto => CompanyService.SetupCheckPoint(checkPointDto), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> SetupEnterLeavePoint([FromBody] AuthorizedDto<EnterLeavePointDto> dto)
        {
            return await Execute(enterLeavePointDto => CompanyService.SetupEnterLeavePoint(enterLeavePointDto), dto);
        }
    }
}
