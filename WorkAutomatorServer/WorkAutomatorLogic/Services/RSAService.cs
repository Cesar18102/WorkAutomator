using System;
using System.Text;
using System.Security.Cryptography;

using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class RSAService : IAsymmetricEncryptionService
    {
        public string Decrypt(string encryptedText, AsymmetricAlgorithm algorithm)
        {
            RSA rsa = (RSA)algorithm;

            byte[] encryptedData = Convert.FromBase64String(encryptedText);
            byte[] decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(decryptedData);
        }

        public string Encrypt(string text, AsymmetricAlgorithm algorithm)
        {
            RSA rsa = (RSA)algorithm;

            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] encryptedData = rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(encryptedData);
        }
    }
}
