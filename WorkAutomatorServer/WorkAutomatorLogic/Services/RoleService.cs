using System;
using System.Threading.Tasks;

using WorkAutomatorLogic.Models.Roles;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class RoleService : ServiceBase, IRoleService
    {
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
