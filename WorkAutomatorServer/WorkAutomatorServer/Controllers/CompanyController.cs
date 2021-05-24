using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

using Autofac;

using WorkAutomatorServer.Dto;
using WorkAutomatorServer.Aspects;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;
using WorkAutomatorLogic.Models.Permission;

namespace WorkAutomatorServer.Controllers
{
    public class CompanyController : ControllerBase
    {
        private static IPermissionService PermissionService = LogicDependencyHolder.Dependencies.Resolve<IPermissionService>();

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

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> TestCreatePermission([FromBody] SessionDto session)
        {
            return await Execute(() => PermissionService.CreateDbPermission(
                new PermissionDbModel(InteractionDbType.CREATE, DbTable.Company),
                session.UserId
            ));
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> TestGrantPermission([FromBody] AuthorizedDto<IdDto> session)
        {
            return await Execute(() => PermissionService.GrantPermission(
                new PermissionDbModel(InteractionDbType.CREATE, DbTable.Company),
                session.Session.UserId, session.Data.Id.Value
            ));
        }
    }
}
