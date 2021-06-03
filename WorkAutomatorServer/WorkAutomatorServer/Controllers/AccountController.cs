using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

using Autofac;

using Dto;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;

using WorkAutomatorServer.Aspects;
using System.Web.Http.Cors;

namespace WorkAutomatorServer.Controllers
{
    [EnableCors("*", "*", "*")]
    public class AccountController : ControllerBase
    {
        private static IAccountService AccountService = LogicDependencyHolder.Dependencies.Resolve<IAccountService>();

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.Id")]
        public async Task<HttpResponseMessage> Get([FromBody] AuthorizedDto<AccountDto> dto)
        {
            return await Execute(acc => AccountService.Get(acc), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.Id", "dto.Data.FirstName", "dto.Data.LastName")]
        public async Task<HttpResponseMessage> Update([FromBody] AuthorizedDto<AccountDto> dto)
        {
            return await Execute(acc => AccountService.UpdateProfile(acc), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> AddBoss([FromBody] AuthorizedDto<SetRemoveBossDto> dto)
        {
            return await Execute(boss => AccountService.AddBoss(boss), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> RemoveBoss([FromBody] AuthorizedDto<SetRemoveBossDto> dto)
        {
            return await Execute(boss => AccountService.RemoveBoss(boss), dto);
        }
    }
}
