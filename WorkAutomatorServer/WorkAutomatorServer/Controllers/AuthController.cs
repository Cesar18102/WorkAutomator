using System.Web.Http;
using System.Net.Http;
using System.Threading.Tasks;

using Autofac;

using Dto;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;
using System.Web.Http.Cors;

namespace WorkAutomatorServer.Controllers
{
    //[EnableCors("*", "*", "*")]
    public class AuthController : ControllerBase
    {
        private static IAuthService AuthService = LogicDependencyHolder.Dependencies.Resolve<IAuthService>();

        [HttpPost]
        public async Task<HttpResponseMessage> SignUp([FromBody] SignUpDto dto)
        {
            return await Execute(signUpDto => AuthService.SignUp(signUpDto), dto);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> LogIn([FromBody] LogInDto dto)
        {
            return await Execute(logInDto => AuthService.LogIn(logInDto), dto);
        }
    }
}
