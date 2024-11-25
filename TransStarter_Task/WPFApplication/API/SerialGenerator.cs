using System.Security.Cryptography;

namespace TransStarter_Task.WPFApplication.API;
internal class SerialGenerator
{
    internal static string Generate()
    {
        var code = SHA1.Create();
        return code.ToString() ?? "NO CODE";
    }
}