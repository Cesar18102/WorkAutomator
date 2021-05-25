using Autofac;

using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorServer.Controllers
{
    public class AdminController : ControllerBase
    {
        IInitService InitService = LogicDependencyHolder.Dependencies.Resolve<IInitService>();

        [HttpPost]
        /*[WireHeadersAspect]
        [AuthorizedAspect]*/
        public async Task<HttpResponseMessage> Init()
        {
            return await Execute(async () =>
            {
                await InitService.InitDbPermissions();
                await InitService.InitDefaultRoles();
                await InitService.InitDataTypes();
                await InitService.InitVisualizerTypes();
            });
        }
    }
}
