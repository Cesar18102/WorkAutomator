using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Autofac;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorServer.Controllers
{
    //[EnableCors("*", "*", "*")]
    public class KeyController : ControllerBase
    {
        private static IAsymmetricEncryptionService AsymmetricEncryptionService = LogicDependencyHolder.Dependencies.Resolve<IAsymmetricEncryptionService>();

        [HttpGet]
        public async Task<HttpResponseMessage> GetPublicAsymmetricKey()
        {
            return await Execute(
                () => AsymmetricEncryptionService.GetNewPublicKey()
            );
        }
    }
}
