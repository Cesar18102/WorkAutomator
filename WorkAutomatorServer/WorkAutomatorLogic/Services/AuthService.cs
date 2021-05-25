using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

using Autofac;

using Dto;

using WorkAutomatorLogic.ServiceInterfaces;
using WorkAutomatorLogic.Exceptions;

using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.Models.Roles;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;

namespace WorkAutomatorLogic.Services
{
    internal class AuthService : ServiceBase, IAuthService
    {
        private static IAsymmetricEncryptionService EncryptionService = LogicDependencyHolder.Dependencies.Resolve<IAsymmetricEncryptionService>();
        private static IKeyService KeyService = LogicDependencyHolder.Dependencies.Resolve<IKeyService>();

        private static IHashingService HashingService = LogicDependencyHolder.Dependencies.Resolve<IHashingService>();
        private static ISessionService SessionService = LogicDependencyHolder.Dependencies.Resolve<ISessionService>();

        private static Regex PasswordPattern = new Regex("^[A-Za-z0-9]{8,32}$");

        public async Task<AccountModel> SignUp(SignUpDto signUpForm)
        {
            return await base.Execute<AccountModel>(async () =>
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IRepo<AccountEntity> accountRepo = db.GetRepo<AccountEntity>();

                    if (await accountRepo.FirstOrDefault(acc => acc.login == signUpForm.Login) != null)
                        throw new LoginDuplicationException();

                    PublicKeyModel publicKeyModel = signUpForm.PublicKey.ToModel<PublicKeyModel>();
                    string password = null;

                    try
                    {
                        if (signUpForm.PublicKey == null)
                            throw new InvalidKeyException();

                        AsymmetricAlgorithm algorithm = KeyService.GetKeyPair(publicKeyModel);
                        password = EncryptionService.Decrypt(signUpForm.PasswordEncrypted, algorithm);
                    }
                    catch (KeyNotFoundException) { throw new InvalidKeyException(); }
                    catch (FormatException) { throw new PostValidationException("Password format was not match Base64"); }
                    catch (CryptographicException) { throw new WrongEncryptionException("Password"); }

                    if (!PasswordPattern.IsMatch(password))
                        throw new InvalidPasswordException();

                    KeyService.DestroyKeyPair(publicKeyModel);

                    AccountEntity accountEntity = new AccountEntity()
                    {
                        login = signUpForm.Login,
                        password = HashingService.GetHashHex(password),
                        first_name = signUpForm.FirstName,
                        last_name = signUpForm.LastName
                    };

                    string authorizedRoleName = DefaultRoles.AUTHORIZED.ToName();
                    accountEntity.Roles.Add(
                        await db.GetRepo<RoleEntity>().FirstOrDefault(
                            role => role.is_default && role.name == authorizedRoleName
                        )
                    );

                    AccountEntity inserted = await accountRepo.Create(accountEntity);
                    await db.Save();

                    return inserted.ToModel<AccountModel>();
                }
            });
        }

        public async Task<SessionModel> LogIn(LogInDto logInForm)
        {
            return await base.Execute<SessionModel>(async () =>
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    AccountEntity account = await db.GetRepo<AccountEntity>().FirstOrDefault(
                        acc => acc.login == logInForm.Login
                    );

                    if (account == null)
                        throw new NotFoundException("Account");

                    string saltedPassword = account.password + logInForm.Salt;
                    string saltedPasswordHash = HashingService.GetHashHex(saltedPassword);

                    if (logInForm.PasswordSalted.ToUpper() != saltedPasswordHash.ToUpper())
                        throw new WrongPasswordException();

                    return SessionService.CreateSessionFor(account.id);
                }
            });
        }
    }
}
