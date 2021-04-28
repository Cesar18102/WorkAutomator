using BusinessLogic.Models;
using System.Threading.Tasks;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    internal interface IRoleCheckService
    {
        Task<bool> IsInRole(RoleEnum role, int userId);
        Task<RoleEnum> GetRole(int userId, int companyId);
        RoleEnum[] GetUpcheckingRoles(RoleEnum role);
    }
}
