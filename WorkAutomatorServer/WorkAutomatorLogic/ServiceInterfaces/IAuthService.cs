using System.Threading.Tasks;

using Dto;

using WorkAutomatorLogic.Models;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IAuthService
    {
        Task<AccountModel> SignUp(SignUpDto signUpForm);
        Task<SessionModel> LogIn(LogInDto logInForm);
    }
}
