using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Set, Nouns.Robot)]
    public class SetRobot : AuthenticatedCmdlet
    {

        [Parameter]
        public Robot Robot { get; set; }

        [Parameter]
        public string Password { get; set; }


        protected override void ProcessRecord()
        {
            var robot = new RobotDto
            {
                Id = Robot.Id,
                Name = Robot.Name,
                MachineName = Robot.MachineName,
                LicenseKey = Robot.LicenseKey,
                Username = Robot.Username,
                Password = Password,
                Description = Robot.Description,
            };

            RobotDtoType type;
            if (Enum.TryParse<RobotDtoType>(Robot.Type, out type))
            {
                robot.Type = type;
            }

            Api.Robots.PutById(robot.Id.Value, robot);
        }
    }
}
