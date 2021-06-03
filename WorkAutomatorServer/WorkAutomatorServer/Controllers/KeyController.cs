using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

using Autofac;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;
using System.Web.Http.Cors;

namespace WorkAutomatorServer.Controllers
{
    //[EnableCors("*", "*", "*")]
    public class KeyController : ControllerBase
    {
        private static IKeyService KeyService = LogicDependencyHolder.Dependencies.Resolve<IKeyService>();

        [HttpGet]
        public async Task<HttpResponseMessage> GetPublicAsymmetricKey()
        {
            return await Execute(KeyService.GetNewPublicKey);
        }
    }
}
