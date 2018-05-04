using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.Tenant)]
    public class RemoveTenant: AuthenticatedCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public int? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Tenant", ValueFromPipeline = true)]
        public Tenant Tenant { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.Tenants.DeleteById(Tenant?.Id ?? Id.Value));
        }
    }
}
