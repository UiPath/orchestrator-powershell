using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20183;
using UiPath.Web.Client20184;
using RobotDtoType20181 = UiPath.Web.Client20181.Models.RobotDtoType;
using RobotDtoType20183 = UiPath.Web.Client20183.Models.RobotDtoType;
using RobotDtoType20184 = UiPath.Web.Client20184.Models.RobotDtoType;
using RobotDtoHostingType20183 = UiPath.Web.Client20183.Models.RobotDtoHostingType;
using RobotDto20181 = UiPath.Web.Client20181.Models.RobotDto;
using RobotDto20183 = UiPath.Web.Client20183.Models.RobotDto;
using RobotDto20184 = UiPath.Web.Client20184.Models.RobotDto;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.Robot)]
    public class AddRobot : AuthenticatedCmdlet
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

        [ValidateEnum(typeof(Web.Client20181.Models.RobotDtoType))]
        [Parameter()]
        public string Type { get; set; }

        [ValidateEnum(typeof(RobotDtoHostingType20183))]
        [Parameter]
        public string HostingType { get; set; }

        [ValidateEnum(typeof(Web.Client20184.Models.RobotDtoCredentialType))]
        [Parameter]
        public string CredentialType { get; set; }

        protected override void ProcessRecord()
        {
            if (MyInvocation.BoundParameters.ContainsKey(nameof(CredentialType)))
            {
                AddRobot20184();
            }
            else if (MyInvocation.BoundParameters.ContainsKey(nameof(HostingType)))
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

            ApplyEnumMember<RobotDtoType20181>(Type, type => robot.Type = type);

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
                Description = Description
            };

            ApplyEnumMember<RobotDtoHostingType20183>(Type, hostingType => robot.HostingType = hostingType);
            ApplyEnumMember<RobotDtoType20183>(Type, type => robot.Type = type);

            var dto = HandleHttpOperationException(() => Api_18_3.Robots.Post(robot));
            WriteObject(Robot.FromDto(dto));
        }

        private void AddRobot20184()
        {
            var robot = new RobotDto20184
            {
                Name = Name,
                MachineName = MachineName,
                LicenseKey = LicenseKey,
                Username = Username,
                Password = Password,
                Description = Description,
            };

            ApplyEnumMember<Web.Client20184.Models.RobotDtoCredentialType>(CredentialType, credentialType => robot.CredentialType = credentialType);
            ApplyEnumMember<Web.Client20184.Models.RobotDtoHostingType>(HostingType, hostingType => robot.HostingType = hostingType);
            ApplyEnumMember<RobotDtoType20184>(Type, type => robot.Type = type);

            var dto = HandleHttpOperationException(() => Api_18_4.Robots.Post(robot));
            WriteObject(Robot.FromDto(dto));
        }
    }
}
