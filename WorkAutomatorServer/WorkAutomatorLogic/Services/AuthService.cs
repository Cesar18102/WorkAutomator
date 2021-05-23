using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

using Autofac;

using WorkAutomatorLogic.ServiceInterfaces;
using WorkAutomatorLogic.Exceptions;
using WorkAutomatorLogic.Models;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.RepoInterfaces;
using WorkAutomatorDataAccess.Entities;

namespace WorkAutomatorLogic.Services
{
    internal class AuthService : ServiceBase, IAuthService
    {
        private static IRepo<AccountEntity> AccountRepo = RepoDependencyHolder.ResolveRealRepo<AccountEntity>();

        private static IAsymmetricEncryptionService EncryptionService = LogicDependencyHolder.Dependencies.Resolve<IAsymmetricEncryptionService>();
        private static IKeyService KeyService = LogicDependencyHolder.Dependencies.Resolve<IKeyService>();

        private static IHashingService HashingService = LogicDependencyHolder.Dependencies.Resolve<IHashingService>();
        private static ISessionService SessionService = LogicDependencyHolder.Dependencies.Resolve<ISessionService>();

        private static Regex PasswordPattern = new Regex("^[A-Za-z0-9]{8,32}$");

        public async Task<AccountModel> SignUp(SignUpFormModel signUpForm)
        {
            return await base.Execute<AccountModel>(async () =>
            {
                if (await AccountRepo.FirstOrDefault(acc => acc.login == signUpForm.Login) != null)
                    throw new LoginDuplicationException();

                string password = null;

                try
                {
                    if (signUpForm.PublicKey == null)
                        throw new InvalidKeyException();

                    AsymmetricAlgorithm algorithm = KeyService.GetKeyPair(signUpForm.PublicKey);
                    password = EncryptionService.Decrypt(signUpForm.PasswordEncrypted, algorithm);
                }
                catch (KeyNotFoundException) { throw new InvalidKeyException(); }
                catch (FormatException) { throw new PostValidationException("Password format was not match Base64"); }
                catch (CryptographicException) { throw new WrongEncryptionException("Password"); }

                if (!PasswordPattern.IsMatch(password))
                    throw new InvalidPasswordException();

                KeyService.DestroyKeyPair(signUpForm.PublicKey);

                AccountEntity accountEntity = signUpForm.ToEntity<AccountEntity>();
                accountEntity.password = HashingService.GetHashHex(password);

                AccountEntity inserted = await AccountRepo.Create(accountEntity);
                return inserted.ToModel<AccountModel>();
            });
        }

        public async Task<SessionModel> LogIn(LogInFormModel logInForm)
        {
            return await base.Execute<SessionModel>(async () =>
            {
                AccountEntity account = await AccountRepo.FirstOrDefault(acc => acc.login == logInForm.Login);

                if (account == null)
                    throw new NotFoundException("Account");

                string saltedPassword = account.password + logInForm.Salt;
                string saltedPasswordHash = HashingService.GetHashHex(saltedPassword);

                if (logInForm.PasswordSalted.ToUpper() != saltedPasswordHash.ToUpper())
                    throw new WrongPasswordException();

                return SessionService.CreateSessionFor(account.id);
            });
        }
    }
}
