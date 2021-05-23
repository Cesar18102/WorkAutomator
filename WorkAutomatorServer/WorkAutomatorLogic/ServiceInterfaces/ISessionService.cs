using WorkAutomatorLogic.Models;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface ISessionService
    {
        SessionModel CreateSessionFor(int accountId);
        void CheckSession(SessionCredentialsModel sessionCredentials);
        void TerminateSession(SessionCredentialsModel sessionCredentials);
    }
}
