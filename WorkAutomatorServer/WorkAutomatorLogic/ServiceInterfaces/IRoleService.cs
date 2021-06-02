using Dto;
using System.Threading.Tasks;

using WorkAutomatorLogic.Models.Roles;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IRoleService
    {
        Task<RoleModel> CreateRole(AuthorizedDto<RoleDto> role);
        Task<RoleModel> UpdateRole(AuthorizedDto<RoleDto> role);

        Task GrantRole(AuthorizedDto<GrantUngrantRoleDto> role);
        Task UnGrantRole(AuthorizedDto<GrantUngrantRoleDto> role);
    }
}
