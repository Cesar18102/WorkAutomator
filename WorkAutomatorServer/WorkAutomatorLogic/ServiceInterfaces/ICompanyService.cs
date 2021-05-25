using System.Threading.Tasks;

using WorkAutomatorLogic.Models;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface ICompanyService
    {
        Task<CompanyModel> CreateCompany(UserActionModel<CompanyModel> model);
        Task<CompanyModel> UpdateCompany(UserActionModel<CompanyModel> model);

        Task<CompanyModel> SetupCompanyPlanPoints(UserActionModel<CompanyModel> model);
        //CompanyModel SetupManufactory()
    }
}
