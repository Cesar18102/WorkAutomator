using System.Web.Http;
using System.Net.Http;
using System.Threading.Tasks;

using Autofac;

using WorkAutomatorLogic;
using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.ServiceInterfaces;

using WorkAutomatorServer.Dto;

namespace WorkAutomatorServer.Controllers
{
    //[EnableCors("*", "*", "*")]
    public class AuthController : ControllerBase
    {
        private static IAuthService AuthService = LogicDependencyHolder.Dependencies.Resolve<IAuthService>();

        [HttpPost]
        public async Task<HttpResponseMessage> SignUp([FromBody] SignUpDto signUpDto)
        {
            return await Execute(
                dto => AuthService.SignUp(dto.ToModel<SignUpFormModel>()),
                signUpDto
            );
        }

        [HttpPost]
        public async Task<HttpResponseMessage> LogIn([FromBody] LogInDto logInDto)
        {
            return await Execute(
                dto => AuthService.LogIn(dto.ToModel<LogInFormModel>()),
                logInDto
            );
        }
    }
}
