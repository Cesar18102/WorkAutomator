using Autofac;
using Dto;

using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;
using WorkAutomatorServer.Aspects;

namespace WorkAutomatorServer.Controllers
{
    [EnableCors("*", "*", "*")]
    public class RoleController : ControllerBase
    {
        private static IRoleService RoleService = LogicDependencyHolder.Dependencies.Resolve<IRoleService>();

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.CompanyId")]
        public async Task<HttpResponseMessage> Create([FromBody] AuthorizedDto<RoleDto> dto)
        {
            return await Execute(role => RoleService.CreateRole(role), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.Id")]
        public async Task<HttpResponseMessage> Update([FromBody] AuthorizedDto<RoleDto> dto)
        {
            return await Execute(role => RoleService.UpdateRole(role), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> Grant([FromBody] AuthorizedDto<GrantUngrantRoleDto> dto)
        {
            return await Execute(role => RoleService.GrantRole(role), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> UnGrant([FromBody] AuthorizedDto<GrantUngrantRoleDto> dto)
        {
            return await Execute(role => RoleService.UnGrantRole(role), dto);
        }
    }
}
