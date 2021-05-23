using System.Threading.Tasks;
using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.Models.Permission;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    internal interface IPermissionService
    {
        Task CheckPermission(Interaction interaction);
        Task GrantPermission(DbTable table, DbPermissionType type, int userId);
    }
}
