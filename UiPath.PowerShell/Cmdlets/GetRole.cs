using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Role)]
    public class GetRole : FilteredBaseCmdlet
    {
        [Filter]
        [Parameter]
        public string Name { get; set; }


        // There is no GetById and GetRoles `Id eq 1` does not work correctly
        // Disable it for the moment
        //[Filter]
        //[Parameter]
        //public int? Id { get; set; }

        [Filter]
        [Parameter]
        public string DisplayName { get; set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                filter => Api.Roles.GetRoles(filter: filter, expand: "Permissions").Value,
                dto => Role.FromDto(dto));
        }
    }
}
