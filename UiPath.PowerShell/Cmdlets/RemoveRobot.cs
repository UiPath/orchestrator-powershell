using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.Robot)]
    public class RemoveRobot: AuthenticatedCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public long? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Robot", ValueFromPipeline = true)]
        public Robot Robot { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.Robots.DeleteById(Robot?.Id ?? Id.Value));
        }
    }
}
