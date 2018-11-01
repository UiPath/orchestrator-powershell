using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Process)]
    public class GetProcess: FilteredIdCmdlet
    {

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public bool? IsLatestVersion { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public bool? IsProcessDeleted { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Name { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Description { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public long? EnvironmentId { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Key { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string ProcessId { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string ProcessVersion { get; private set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                filter => Api.Releases.GetReleases(filter: filter).Value,
                id => Api.Releases.GetById(id),
                dto => Process.FromDto(dto));
        }
    }
}
