using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

using Autofac;

using Dto;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;

using WorkAutomatorServer.Aspects;

namespace WorkAutomatorServer.Controllers
{
    public class ManufactoryController : ControllerBase
    {
        private static IManufactoryService ManufactoryService = LogicDependencyHolder.Dependencies.Resolve<IManufactoryService>();

        
    }
}
