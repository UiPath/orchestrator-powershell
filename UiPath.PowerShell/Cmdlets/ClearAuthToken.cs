using System.Management.Automation;
using UiPath.PowerShell.Util;

namespace UiPath.PowerShell.Cmdlets
{
    /// <summary>
    /// <para name="synopsis">Clears the Orchestrator authentication token from the Powershell session.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Clear, Nouns.AuthToken)]
    public class ClearAuthToken: UiPathCmdlet
    {
        protected override void ProcessRecord()
        {
            AuthenticatedCmdlet.SetAuthToken(null);
        }
    }
}
