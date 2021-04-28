using System.Threading.Tasks;

using WorkAutomatorLogic.Models;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IAuthService
    {
        Task<AccountModel> SignUp(SignUpFormModel signUpForm);
        Task<SessionModel> LogIn(LogInFormModel logInForm);
    }
}
