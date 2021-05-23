using WorkAutomatorLogic.Models;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface ICompanyService
    {
        CompanyModel CreateCompany(CompanyModel model);
        CompanyModel SetupCompanyPlanPoints(CompanyModel model);
        //CompanyModel SetupManufactory()
    }
}
