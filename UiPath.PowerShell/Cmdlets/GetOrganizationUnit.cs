using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.OrganizationUnit)]
    public class GetOrganizationUnit: FilteredIdCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "DisplayName", Position = 0)]
        public string DisplayName { get; set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                (filter, top, skip) => Api.OrganizationUnits.GetOrganizationUnits(filter: filter, top: top, skip: skip, count: false),
                id  => Api.OrganizationUnits.GetById(id),
                dto => OrganizationUnit.FromDto(dto));
        }
    }
}
