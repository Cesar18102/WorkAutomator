using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

using WorkAutomatorServer.Dto;
using WorkAutomatorServer.Aspects;

namespace WorkAutomatorServer.Controllers
{
    public class CompanyController : ControllerBase
    {
        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> TestSessionDto([FromBody] SessionDto session)
        {
            return await Execute(() => null);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> TestAuthorizedDto([FromBody] AuthorizedDto<LogInDto> session)
        {
            return await Execute(() => null);
        }
    }
}
