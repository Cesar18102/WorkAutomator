using WorkAutomatorLogic.Models;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IAsymmetricEncryptionService
    {
        PublicKeyModel GetNewPublicKey();
        string Decrypt(string encryptedText, PublicKeyModel publicKey);
        string Encrypt(string text, PublicKeyModel publicKey);
        void DestroyKeyPair(PublicKeyModel publicKey);
    }
}
