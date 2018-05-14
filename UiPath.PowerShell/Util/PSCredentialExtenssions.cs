using System.Management.Automation;
using System.Net;

namespace UiPath.PowerShell.Util
{
    internal static class PSCredentialExtenssions
    {
        public static string ExtractPassword(this PSCredential ps)
        {
            return new NetworkCredential(string.Empty, ps.Password).Password;
        }
    }
}
