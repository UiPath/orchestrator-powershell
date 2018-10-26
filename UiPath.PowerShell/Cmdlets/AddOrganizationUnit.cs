using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.OrganizationUnit)]
    public class AddOrganizationUnit: AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string DisplayName { get; set; }

        protected override void ProcessRecord()
        {
            var postDto = new OrganizationUnitDto
            {
                DisplayName = DisplayName
            };

            var response = HandleHttpOperationException(() => Api.OrganizationUnits.Post(postDto));
            WriteObject(OrganizationUnit.FromDto(response));
        }
    }
}
