using System.Threading.Tasks;

using Dto;

using WorkAutomatorLogic.Models;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface ICompanyService
    {
        Task<CompanyModel> GetCompany(AuthorizedDto<CompanyIdDto> model);

        Task<CompanyModel> CreateCompany(AuthorizedDto<CompanyDto> model);
        Task<CompanyModel> UpdateCompany(AuthorizedDto<CompanyDto> model);

        Task<CompanyModel> HireMember(AuthorizedDto<FireHireDto> model);
        Task<CompanyModel> FireMember(AuthorizedDto<FireHireDto> model);

        Task<CompanyModel> SetupCompanyPlanPoints(AuthorizedDto<CompanyPlanPointsDto> model);
        Task<CompanyModel> SetupCheckPoint(AuthorizedDto<CheckPointDto> model);
        Task<CompanyModel> SetupEnterLeavePoint(AuthorizedDto<EnterLeavePointDto> model);
    }
}
