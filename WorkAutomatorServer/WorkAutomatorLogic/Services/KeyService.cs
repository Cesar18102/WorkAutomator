using System.Collections.Generic;
using System.Security.Cryptography;

using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class KeyService : IKeyService
    {
        private Dictionary<PublicKeyModel, RSA> Codes = new Dictionary<PublicKeyModel, RSA>();

        public PublicKeyModel GetNewPublicKey()
        {
            RSA rsa = RSA.Create();

            PublicKeyModel publicKey = new PublicKeyModel(
                rsa.ToXmlString(false)
            );

            Codes.Add(publicKey, rsa);

            return publicKey;
        }

        public AsymmetricAlgorithm GetKeyPair(PublicKeyModel publicKey)
        {
            return Codes[publicKey];
        }

        public void DestroyKeyPair(PublicKeyModel publicKey)
        {
            Codes[publicKey].Dispose();
            Codes.Remove(publicKey);
        }
    }
}
