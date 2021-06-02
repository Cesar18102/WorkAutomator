using System.Threading.Tasks;

using Dto;

using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.Models.Roles;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IRoleService
    {
        Task<RoleModel> CreateRole(AuthorizedDto<RoleDto> role);
        Task<RoleModel> UpdateRole(AuthorizedDto<RoleDto> role);

        Task<WorkerModel> GrantRole(AuthorizedDto<GrantUngrantRoleDto> role);
        Task<WorkerModel> UnGrantRole(AuthorizedDto<GrantUngrantRoleDto> role);
    }
}
