using System.Management.Automation;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using Environment = UiPath.PowerShell.Models.Environment;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Environment)]
    public class GetEnvironment: FilteredCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Name { get; set; }

        [Filter]
        [ValidateSet("Dev", "Test", "Prod")]
        [Parameter(ParameterSetName = "Filter")]
        public string Type { get; set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                filter => Api.Environments.GetEnvironments(filter: filter).Value,
                id => Api.Environments.GetById(id),
                dto => Environment.FromDto(dto));
        }
    }
}
