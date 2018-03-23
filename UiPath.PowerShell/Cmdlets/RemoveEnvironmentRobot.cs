using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;
using Environment = UiPath.PowerShell.Models.Environment;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.EnvironmentRobot)]
    public class RemoveEnvironmentRobot: AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public long? EnvironmentId { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "Id")]
        public long? RobotId { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Values")]
        public Environment Environment { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "Values")]
        public Robot Robot { get; set; }

        protected override void ProcessRecord()
        {
            Api.Environments.RemoveRobotById(Environment?.Id ?? EnvironmentId.Value, new RemoveRobotParameters
            {
                RobotId = (Robot?.Id ?? RobotId.Value).ToString()
            });
        }
    }
}
