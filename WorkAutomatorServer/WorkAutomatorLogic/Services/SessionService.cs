using System;
using System.Collections.Generic;

using Autofac;

using WorkAutomatorLogic.Exceptions;

using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class SessionService : ISessionService
    {
        private const int SESSION_DURATION = 3600;

        private static IDictionary<int, SessionModel> Sessions = new Dictionary<int, SessionModel>();
        private static IHashingService Hasher = LogicDependencyHolder.Dependencies.Resolve<IHashingService>();

        private string GenerateToken()
        {
            string seed = Guid.NewGuid().ToString();
            return Hasher.GetHashHex(seed);
        }

        public SessionModel CreateSessionFor(int accountId)
        {
            if (Sessions.ContainsKey(accountId))
                Sessions.Remove(accountId);

            string token = GenerateToken();
            DateTime expires = DateTime.Now.AddSeconds(SESSION_DURATION);

            SessionModel session = new SessionModel()
            {
                UserId = accountId,
                Token = token,
                ExpiredAt = expires
            };

            Sessions.Add(accountId, session);
            return session;
        }

        public void CheckSession(SessionCredentialsModel sessionCredentials)
        {
            int userId = sessionCredentials.UserId;
            if (!Sessions.ContainsKey(userId))
                throw new SessionNotFoundException();

            SessionModel session = Sessions[userId];

            string originalTokenSalted = Hasher.GetHashHex(session.Token + sessionCredentials.Salt);
            if (originalTokenSalted.ToUpper() != sessionCredentials.SessionTokenSalted.ToUpper())
                throw new WrongSessionTokenException();

            if (session.ExpiredAt < DateTime.Now)
            {
                Sessions.Remove(userId);
                throw new SessionExpiredException();
            }

            double secondsLeft = (session.ExpiredAt - DateTime.Now).TotalSeconds;

            if (secondsLeft < SESSION_DURATION / 2)
                session.ExpiredAt = session.ExpiredAt.AddSeconds(SESSION_DURATION - secondsLeft);
        }

        public void TerminateSession(SessionCredentialsModel sessionCredentials)
        {
            CheckSession(sessionCredentials);
            Sessions.Remove(sessionCredentials.UserId);
        }
    }
}
