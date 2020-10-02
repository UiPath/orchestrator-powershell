using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20183;

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
            if (Supports(OrchestratorProtocolVersion.V19_10))
            {
                ProcessImpl(
                    (filter, top, skip) => HandleHttpResponseException(() => Api_19_10.Releases.GetReleasesWithHttpMessagesAsync(filter: filter, top: top, skip: skip, count: false)),
                    id => HandleHttpResponseException(() => Api_19_10.Releases.GetByIdWithHttpMessagesAsync(id)),
                    dto => Process.FromDto(dto));

            }
            else
            {
                ProcessImpl(
                    (filter, top, skip) => Api_18_3.Releases.GetReleases(filter: filter, top: top, skip: skip, count: false),
                    id => Api_18_3.Releases.GetById(id),
                    dto => Process.FromDto(dto));
            }
        }
    }
}
