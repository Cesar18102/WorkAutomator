using System.Threading.Tasks;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IInitService
    {
        Task InitDbPermissions();
        Task InitDefaultRoles();
        Task InitDataTypes();
        Task InitVisualizerTypes();
    }
}
