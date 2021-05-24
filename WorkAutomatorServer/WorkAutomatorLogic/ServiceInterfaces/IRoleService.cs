using System.Threading.Tasks;

using WorkAutomatorLogic.Models;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IRoleService
    {
        Task<RoleModel> CreateRole(RoleModel role);
        Task<RoleModel> UpdateRole(RoleModel role);
    }
}
