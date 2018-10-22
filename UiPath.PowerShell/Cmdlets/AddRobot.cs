using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;
using RobotDtoType20181 = UiPath.Web.Client.Models.RobotDtoType;
using RobotDtoType20183 = UiPath.Web.Client20183.Models.RobotDtoType;
using RobotDtoHostingType20183 = UiPath.Web.Client20183.Models.RobotDtoHostingType;
using RobotDto20181 = UiPath.Web.Client.Models.RobotDto;
using RobotDto20183 = UiPath.Web.Client20183.Models.RobotDto;
using UiPath.Web.Client20183;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.Robot)]
    public class AddRobot: AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Name { get; private set; }

        [Parameter()]
        public string MachineName { get; private set; }

        [Parameter()]
        public string LicenseKey { get; private set; }

        [Parameter(Mandatory = true)]
        public string Username { get; private set; }

        //[Credential]
        [Parameter()]
        public string Password { get; private set; }

        [Parameter()]
        public string Description { get; private set; }

        [ValidateEnum(typeof(Web.Client.Models.RobotDtoType))]
        [Parameter()]
        public string Type { get; set; }

        [ValidateEnum(typeof(RobotDtoHostingType20183))]
        [Parameter]
        public string HostingType { get; set; }

        protected override void ProcessRecord()
        {
            if (MyInvocation.BoundParameters.ContainsKey(nameof(HostingType)))
            {
                AddRobot20183();
            }
            else
            {
                AddRobot20181();
            }
        }

        private void AddRobot20181()
        { 
            var robot = new RobotDto20181
            {
                Name = Name,
                MachineName = MachineName,
                LicenseKey = LicenseKey,
                Username = Username,
                Password = Password,
                Description = Description,
            };

            RobotDtoType20181 type;
            if (Enum.TryParse<RobotDtoType20181>(Type, out type))
            {
                robot.Type = type;
            }

            var dto = HandleHttpOperationException(() => Api.Robots.Post(robot));
            WriteObject(Robot.FromDto(dto));
        }

        private void AddRobot20183()
        {
            var robot = new RobotDto20183
            {
                Name = Name,
                MachineName = MachineName,
                LicenseKey = LicenseKey,
                Username = Username,
                Password = Password,
                Description = Description,
                HostingType = (RobotDtoHostingType20183)Enum.Parse(typeof(RobotDtoHostingType20183), HostingType)
            };

            RobotDtoType20183 type;
            if (Enum.TryParse<RobotDtoType20183>(Type, out type))
            {
                robot.Type = type;
            }

            var dto = HandleHttpOperationException(() => Api_18_3.Robots.Post(robot));
            WriteObject(Robot.FromDto(dto));
        }
    }
}
