using System.Net;

namespace UiPath.PowerShell.Util
{
    internal class SecurityProtocolFix
    {
        internal static bool Ignored = false;

        static SecurityProtocolFix()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
        }
    }
}
