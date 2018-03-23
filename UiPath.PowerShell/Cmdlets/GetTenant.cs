using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Tenant)]
    public class GetTenant: FilteredCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Name { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string AdminName { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string AdminSurname { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string AdminEmailAddress { get; private set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                filter => Api.Tenants.GetTenants(filter: filter).Value,
                id => Api.Tenants.GetById((int)id),
                dto => Tenant.ForDto(dto));
        }
    }
}
