using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20183;
using UiPath.Web.Client20204;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Robot)]
    public class GetRobot:FilteredIdCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Name { get; set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string MachineName { get; set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string LicenseKey { get; set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Username { get; set; }

        [Filter]
        [Parameter(ParameterSetName="Filter")]
        public string Type { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string HostingType { get; set; }

        protected override void ProcessRecord()
        {
            if (Supports(OrchestratorProtocolVersion.V20_4))
            {
                ProcessImpl(
                   (filter, top, skip) => Api_20_4.Robots.Get(filter: filter, top: top, skip: skip, count: false),
                    id => Api_20_4.Robots.GetById(id),
                    dto => Robot.FromDto(dto));

            }
            else if (Supports(OrchestratorProtocolVersion.V19_10))
            {
                ProcessImpl(
                   (filter, top, skip) => HandleHttpResponseException(() => Api_19_10.Robots.GetRobotsWithHttpMessagesAsync(filter: filter, top: top, skip: skip, count: false)),
                    id => HandleHttpResponseException(() => Api_19_10.Robots.GetByIdWithHttpMessagesAsync(id)),
                    dto => Robot.FromDto(dto));

            }
            else if (Supports(OrchestratorProtocolVersion.V18_3) || 
                MyInvocation.BoundParameters.ContainsKey(nameof(HostingType)))
            {
                ProcessImpl(
                    (filter, top, skip) => Api_18_3.Robots.GetRobots(filter: filter, top: top, skip: skip, count: false),
                    id => Api_18_3.Robots.GetById(id),
                    dto => Robot.FromDto(dto));
            }
            else
            {
                ProcessImpl(
                    (filter, top, skip) => Api.Robots.GetRobots(filter: filter, top: top, skip: skip, count: false),
                    id => Api.Robots.GetById(id),
                    dto => Robot.FromDto(dto));
            }
        }
    }
}
