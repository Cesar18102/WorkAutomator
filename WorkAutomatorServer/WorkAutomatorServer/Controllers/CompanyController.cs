using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

using Autofac;

using WorkAutomatorServer.Dto;
using WorkAutomatorServer.Aspects;

using WorkAutomatorLogic;
using WorkAutomatorLogic.Models;
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
            return await Execute(
                company => CompanyService.CreateCompany(
                    company.ToModel<CompanyDto, CompanyModel>()
                ), dto
            );
        }
    }
}
