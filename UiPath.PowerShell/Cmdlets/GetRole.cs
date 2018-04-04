using System.Linq;
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
            // We need to extra filter on Name/DisplayName because the API retruns 'Like %Name%' instead of 'eq Name'
            ProcessImpl(
                filter => Api.Roles.GetRoles(filter: filter, expand: "Permissions").Value
                .Where(r => string.IsNullOrWhiteSpace(Name) ? true : r.Name.Equals(Name))
                .Where(r => string.IsNullOrWhiteSpace(DisplayName) ? true : r.DisplayName.Equals(DisplayName))
                .ToList(),
                dto => Role.FromDto(dto));
        }
    }
}
