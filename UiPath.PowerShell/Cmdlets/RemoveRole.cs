using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.Role)]
    public class RemoveRole : AuthenticatedCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public int? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Role", ValueFromPipeline = true)]
        public Role Role { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.Roles.DeleteById(Role?.Id ?? Id.Value));
        }
    }
}
