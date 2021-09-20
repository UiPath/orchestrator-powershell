using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.ProcessSchedule)]
    public class GetProcessSchedule : FilteredIdCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Name { get; internal set; }

        protected override void ProcessRecord()
        {
            if (Supports(OrchestratorProtocolVersion.V19_10))
            {
                ProcessImpl(
                    (filter, top, skip) => HandleHttpResponseException(() => Api_19_10.ProcessSchedules.GetProcessSchedulesWithHttpMessagesAsync(filter: filter, top: top, skip: skip, count: false)),
                    id => HandleHttpResponseException(() => Api_19_10.ProcessSchedules.GetByIdWithHttpMessagesAsync(id)),
                    dto => ProcessSchedule.FromDto(dto));
            }
            else
            {
                ProcessImpl(
                    (filter, top, skip) => Api.ProcessSchedules.GetProcessSchedules(filter: filter, top: top, skip: skip, count: false),
                    id => Api.ProcessSchedules.GetById(id),
                    dto => ProcessSchedule.FromDto(dto));
            }
        }
    }
}
