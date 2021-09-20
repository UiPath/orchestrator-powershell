using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using UiPath.Web.Client20194.Models;

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

        [ValidateEnum(typeof(RobotDtoType))]
        [Parameter()]
        public string Type { get; set; }

        [ValidateEnum(typeof(RobotDtoHostingType))]
        [Parameter]
        public string HostingType { get; set; }

        [ValidateEnum(typeof(RobotDtoCredentialType))]
        [Parameter]
        public string CredentialType { get; set; }

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

            ApplyEnumMember<RobotDtoCredentialType>(CredentialType, credentialType => robot.CredentialType = credentialType);
            ApplyEnumMember<RobotDtoHostingType>(HostingType, hostingType => robot.HostingType = hostingType);
            ApplyEnumMember<RobotDtoType>(Type, type => robot.Type = type);

            var dto = HandleHttpOperationException(() => Api_19_4.Robots.Post(robot));
            WriteObject(Robot.FromDto(dto));
        }
    }
}
