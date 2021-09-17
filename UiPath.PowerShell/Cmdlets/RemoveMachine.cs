using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.Machine)]
    public class RemoveMachine : AuthenticatedCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public long? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Machine", ValueFromPipeline = true)]
        public Machine Machine { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.Machines.DeleteById(Machine?.Id ?? Id.Value));
        }
    }
}
