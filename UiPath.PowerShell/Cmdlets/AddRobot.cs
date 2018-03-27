using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.Robot)]
    public class AddRobot: AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Name { get; private set; }

        [Parameter(Mandatory = true)]
        public string MachineName { get; private set; }

        [Parameter(Mandatory = true)]
        public string LicenseKey { get; private set; }

        [Parameter(Mandatory = true)]
        public string Username { get; private set; }

        //[Credential]
        [Parameter()]
        public string Password { get; private set; }

        [Parameter()]
        public string Description { get; private set; }

        [ValidateEnum(typeof(RobotDtoType))]
        [Parameter()]
        public string Type { get; set; }

        protected override void ProcessRecord()
        {
            var robot = new RobotDto
            {
                Name = Name,
                MachineName = MachineName,
                LicenseKey = LicenseKey,
                Username = Username,
                Password = Password,
                Description = Description,
            };

            RobotDtoType type;
            if (Enum.TryParse<RobotDtoType>(Type, out type))
            {
                robot.Type = type;
            }

            var dto = Api.Robots.Post(robot);
            WriteObject(Robot.FromDto(dto));
        }
    }
}
