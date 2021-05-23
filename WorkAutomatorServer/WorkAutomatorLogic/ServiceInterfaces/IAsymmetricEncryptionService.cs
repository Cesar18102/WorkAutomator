using System.Security.Cryptography;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IAsymmetricEncryptionService
    {
        string Decrypt(string encryptedText, AsymmetricAlgorithm algorithm);
        string Encrypt(string text, AsymmetricAlgorithm algorithm);
    }
}
