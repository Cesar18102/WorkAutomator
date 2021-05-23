using System.Security.Cryptography;

using WorkAutomatorLogic.Models;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IKeyService
    {
        PublicKeyModel GetNewPublicKey();
        AsymmetricAlgorithm GetKeyPair(PublicKeyModel publicKey);
        void DestroyKeyPair(PublicKeyModel publicKey);
    }
}
