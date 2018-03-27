using System.Management.Automation;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;
using Environment = UiPath.PowerShell.Models.Environment;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Environment)]
    public class GetEnvironment: FilteredIdCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Name { get; set; }

        [Filter]
        [ValidateEnum(typeof(EnvironmentDtoType))]
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
