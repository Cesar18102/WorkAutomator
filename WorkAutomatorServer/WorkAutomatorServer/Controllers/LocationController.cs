using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

using Autofac;

using Dto.Interaction;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorServer.Controllers
{
    public class LocationController : ControllerBase
    {
        private static ILocationService LocationService = LogicDependencyHolder.Dependencies.Resolve<ILocationService>();

        [HttpPost]
        public async Task<HttpResponseMessage> TryCheckout([FromBody] CheckoutDto dto)
        {
            return await Execute(d => LocationService.TryCheckout(d), dto);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> TryEnter([FromBody] EnterLeaveDto dto)
        {
            return await Execute(d => LocationService.TryEnter(d), dto);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> TryLeave([FromBody] EnterLeaveDto dto)
        {
            return await Execute(d => LocationService.TryLeave(d), dto);
        }
    }
}
