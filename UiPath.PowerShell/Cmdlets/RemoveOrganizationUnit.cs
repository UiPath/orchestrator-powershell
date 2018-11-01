using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.OrganizationUnit)]
    public class RemoveOrganizationUnit: AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public OrganizationUnit OrganizationUnit { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.OrganizationUnits.DeleteById(OrganizationUnit.Id));
        }
    }
}
