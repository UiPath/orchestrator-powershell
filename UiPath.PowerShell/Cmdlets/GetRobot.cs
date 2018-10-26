using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20183;

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

        [ValidateEnum(typeof(Web.Client20181.Models.RobotDtoType))]
        [Filter]
        [Parameter(ParameterSetName="Filter")]
        public string Type { get; private set; }

        [ValidateEnum(typeof(Web.Client20183.Models.RobotDtoHostingType))]
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string HostingType { get; set; }

        protected override void ProcessRecord()
        {
            if (Supports(OrchestratorProtocolVersion.V18_3) || 
                MyInvocation.BoundParameters.ContainsKey(nameof(HostingType)))
            {
                ProcessImpl(
                    filter => Api_18_3.Robots.GetRobots(filter: filter).Value,
                    id => Api_18_3.Robots.GetById(id),
                    dto => Robot.FromDto(dto));
            }
            else
            {
                ProcessImpl(
                    filter => Api.Robots.GetRobots(filter: filter).Value,
                    id => Api.Robots.GetById(id),
                    dto => Robot.FromDto(dto));
            }
        }
    }
}
