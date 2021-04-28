using System.Text;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    internal interface IHashingService
    {
        string GetHashHex(string text, Encoding encoding = null);
        string GetHashBase64(string text, Encoding encoding = null);
    }
}
