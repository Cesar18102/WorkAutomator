using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using NUnit.Framework;

using WorkAutomatorLogic;
using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.Exceptions;
using WorkAutomatorLogic.ServiceInterfaces;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.RepoInterfaces;
using WorkAutomatorDataAccess.Entities;

namespace BusinessLogicTests
{
    public class AuthServiceTests
    {
        protected static IKeyService KeyService = LogicDependencyHolder.Dependencies.Resolve<IKeyService>();
        protected static IAuthService AuthService = LogicDependencyHolder.Dependencies.Resolve<IAuthService>();

        protected static IRepo<AccountEntity> AccountRepo = RepoDependencyHolder.Dependencies.Resolve<IRepo<AccountEntity>>();

        [Test]
        public async Task InsertInvalidTest()
        {
            PublicKeyModel key = KeyService.GetNewPublicKey();

            RSA rsa = RSA.Create(
                new RSAParameters()
                {
                    Exponent = Convert.FromBase64String(key.Exponent),
                    Modulus = Convert.FromBase64String(key.Modulus)
                }
            );

            SignUpFormModel signUpForm = new SignUpFormModel()
            {
                FirstName = "testFirstName",
                LastName = "testLastName",
                Login = "testLongLogintestLongLogintestLongLogintestLongLogintestLongLogintestLongLogin" +
                         "testLongLogintestLongLogintestLongLogintestLongLogintestLongLogintestLongLogin" +
                         "testLongLogintestLongLogintestLongLogintestLongLogintestLongLogintestLongLogintestLongLogintestLongLogin" + new Guid().ToString(), //260
                PasswordEncrypted = Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes("123456789"), RSAEncryptionPadding.Pkcs1)),
                PublicKey = key
            };


            try
            {
                AccountModel account = await AuthService.SignUp(signUpForm);
                await AccountRepo.Delete(account.Id);
                Assert.Fail();
            }
            catch(DataValidationException ex)
            {
                Assert.Pass();
            }
        }
    }
}