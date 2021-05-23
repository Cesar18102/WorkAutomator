using System.Threading.Tasks;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;

using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class PermissionService : IPermissionService
    {
        private static IRepo<AccountEntity> AccountRepo = RepoDependencyHolder.ResolveRealRepo<AccountEntity>();

        public Task CheckDbPermission(DbTable table, DbPermissionType type, int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}