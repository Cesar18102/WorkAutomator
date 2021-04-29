using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

using Autofac;

using WorkAutomatorLogic.ServiceInterfaces;
using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.Exceptions;

namespace WorkAutomatorLogic.Services
{
    internal class AuthService : IAuthService
    {
        //private static IAccountRepo AccountRepo = 
        //    DataAccessDependencyHolder.Dependencies.Resolve<IAccountRepo>();

        private static IAsymmetricEncryptionService EncryptionService = LogicDependencyHolder.Dependencies.Resolve<IAsymmetricEncryptionService>();
        private static IHashingService HashingService = LogicDependencyHolder.Dependencies.Resolve<IHashingService>();
        private static ISessionService SessionService = LogicDependencyHolder.Dependencies.Resolve<ISessionService>();

        private static Regex PasswordPattern = new Regex("^[A-Za-z0-9]{8,32}$");

        public async Task<AccountModel> SignUp(SignUpFormModel signUpForm)
        {
            /*if (await AccountRepo.GetByLogin(signUpForm.Login) != null)
                throw new LoginDuplicationException();

            string password = null;

            try { password = EncryptionService.Decrypt(signUpForm.PasswordEncrypted, signUpForm.PublicKey); }
            catch(KeyNotFoundException) { throw new InvalidKeyException(); } 
            catch(FormatException) { throw new PostValidationException("Password format was not match Base64"); }
            catch(CryptographicException) { throw new WrongEncryptionException("Password"); }

            EncryptionService.DestroyKeyPair(signUpForm.PublicKey);

            if (!PasswordPattern.IsMatch(password))
                throw new InvalidPasswordException();

            AccountEntity accountEntity = signUpForm.ToEntity<AccountEntity>();
            accountEntity.PasswordHash = HashingService.GetHashHex(password);

            AccountEntity inserted = await AccountRepo.Insert(accountEntity);

            /return ModelEntityMapper.Mapper.Map<AccountModel>(inserted);*/
            return null;
        }

        public async Task<SessionModel> LogIn(LogInFormModel logInForm)
        {
            /*AccountEntity account = await AccountRepo.FirstOrDefault(acc => acc.Login, logInForm.Login);

            if (account == null)
                throw new AccountNotFoundException();

            string saltedPassword = account.Password + logInForm.Salt;
            string saltedPasswordHash = HashingService.GetHashHex(saltedPassword);

            if (logInForm.PasswordSalted.ToUpper() != saltedPasswordHash.ToUpper())
                throw new WrongPasswordException();

            return SessionService.CreateSessionFor(account.Id);*/
            return null;
        }
    }
}
