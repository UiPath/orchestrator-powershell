using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add,  Nouns.EnvironmentRobot)]
    public class AddEnvironmentRobot: AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true)]
        public Environment Environment { get; set; }

        [Parameter(Mandatory = true)]
        public Robot Robot { get; set; }

        protected override void ProcessRecord()
        {
            Api.Environments.AddRobotById(Environment.Id, new AddRobotParameters
            {
                RobotId = Robot.Id.ToString()
            });
        }
    }
}
