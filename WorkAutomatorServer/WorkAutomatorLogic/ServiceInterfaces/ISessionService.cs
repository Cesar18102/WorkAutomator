using WorkAutomatorLogic.Models;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    internal interface ISessionService
    {
        SessionModel CreateSessionFor(int accountId);
        void CheckSession(SessionCredentialsModel sessionCredentials);
        void TerminateSession(SessionCredentialsModel sessionCredentials);
    }
}
