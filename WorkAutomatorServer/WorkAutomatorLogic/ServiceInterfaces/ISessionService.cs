using Dto;

using WorkAutomatorLogic.Models;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface ISessionService
    {
        SessionModel CreateSessionFor(int accountId);
        void CheckSession(SessionDto sessionCredentials);
        void TerminateSession(SessionDto sessionCredentials);
    }
}
