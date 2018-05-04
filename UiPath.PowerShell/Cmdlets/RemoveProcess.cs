using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.Process)]
    public class RemoveProcess: AuthenticatedCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public long? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Process", ValueFromPipeline = true)]
        public Process Process { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.Releases.DeleteById(Process?.Id ?? Id.Value));
        }
    }
}
