using System.Threading.Tasks;

using Attributes;

using WorkAutomatorLogic.Models.Permission;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IPermissionService
    {
        Task<bool> IsLegal(Interaction interaction);
        Task CheckLegal(Interaction interaction);
        Task<PermissionDbModel[]> GetDbPermissions();
    }
}
