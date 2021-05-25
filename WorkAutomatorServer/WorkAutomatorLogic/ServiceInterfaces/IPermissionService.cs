using System.Threading.Tasks;

using Attributes;

using WorkAutomatorLogic.Models.Permission;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IPermissionService
    {
        Task CreateDbPermission(PermissionDbModel dbPermission, [InitiatorAccountId] int creatorAccountId);

        Task GrantPermission([TableNameParameter] PermissionModelBase permission, [InitiatorAccountId] int grantingByAccountId, int grantingToRoleId);
        Task UnGrantPermission([TableNameParameter] PermissionModelBase permission, [InitiatorAccountId] int grantingByAccountId, int grantingToRoleId);

        Task<bool> IsLegal(Interaction interaction);
        Task CheckLegal(Interaction interaction);
    }
}
