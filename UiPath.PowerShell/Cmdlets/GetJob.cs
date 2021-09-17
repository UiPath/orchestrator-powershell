using System.Management.Automation;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Job)]
    public class GetJob: FilteredIdCmdlet
    {
        [Filter]
        [ValidateEnum(typeof(UiPath.Web.Client20204.Models.JobDtoSourceType))]
        [Parameter(ParameterSetName = "Filter")]
        public string Source { get; set; }

        [Filter]
        [ValidateEnum(typeof(UiPath.Web.Client20204.Models.JobDtoState))]
        [Parameter(ParameterSetName = "Filter")]
        public string State { get; set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string ReleaseName { get; set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                (filter, top, skip) => Api_19_4.Jobs.GetJobs(filter: filter, top: top, skip: skip, count: false),
                id => Api_19_4.Jobs.GetById(id),
                dto => Models.Job.FromDto(dto));
        }
    }
}
