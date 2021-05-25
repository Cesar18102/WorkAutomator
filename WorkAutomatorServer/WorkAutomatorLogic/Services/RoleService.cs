using System;
using System.Threading.Tasks;

using WorkAutomatorDataAccess.Entities;

using WorkAutomatorLogic.Models.Roles;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class RoleService : ServiceBase, IRoleService
    {
        public async Task<RoleEntity> GetDefaultRole(DefaultRoles defaultRole)
        {
            return null;
            /*string name = defaultRole.ToName();

            return await RoleRepo.FirstOrDefault(
                role => role.is_default && role.name == name
            );*/
        }

        public Task<RoleModel> CreateRole(RoleModel role)
        {
            throw new NotImplementedException();
        }

        public Task<RoleModel> UpdateRole(RoleModel role)
        {
            throw new NotImplementedException();
        }
    }
}
