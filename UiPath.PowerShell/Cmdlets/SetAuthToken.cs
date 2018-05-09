using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;

namespace UiPath.PowerShell.Cmdlets
{
    /// <summary>
    /// <para name="synopsis">Sets the Powershell session Orchestrator authentication token.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Set, Nouns.AuthToken)]
    public class SetAuthToken: UiPathCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "AuthToken", Position = 0, ValueFromPipeline = true)]
        public AuthToken AuthToken { get; set; }

        protected override void ProcessRecord()
        {
            if (AuthToken != null)
            {
                AuthenticatedCmdlet.SetAuthToken(AuthToken);
            }
        }
    }
}
