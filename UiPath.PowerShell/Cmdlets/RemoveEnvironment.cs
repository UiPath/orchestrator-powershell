using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.Environment)]
    public class RemoveEnvironment: AuthenticatedCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public long? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Environment", ValueFromPipeline = true)]
        public Environment Environment { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.Environments.DeleteById(Environment?.Id ?? Id.Value));
        }
    }
}
