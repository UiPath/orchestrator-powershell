using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.Edit, Nouns.Robot)]
    public class EditRobot : AuthenticatedCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public Robot Robot { get; set; }

        [Parameter]
        public string Name { get; private set; }

        [Parameter]
        public string MachineName { get; private set; }

        [Parameter]
        public string LicenseKey { get; private set; }

        [Parameter]
        public string Username { get; private set; }

        //[Credential]
        [Parameter]
        public string Password { get; private set; }

        [Parameter]
        public string Description { get; private set; }

        [ValidateSet("NonProduction", "Attended", "Unattended")]
        [Parameter]
        public string Type { get; set; }

        protected override void ProcessRecord()
        {
            var robot = new RobotDto
            {
                Id = Robot.Id, // bugbug: UI-6895
                Name = Name ?? Robot.Name,
                MachineName = MachineName ?? Robot.MachineName,
                LicenseKey = LicenseKey ?? Robot.LicenseKey,
                Username = Username ?? Robot.Username,
                Password = Password,
                Description = Description ?? Robot.Description,
            };

            RobotDtoType type;
            if (Enum.TryParse<RobotDtoType>(Type, out type))
            {
                robot.Type = type;
            }

            var dto = Api.Robots.PutById(Robot.Id, robot);
            // WriteObject(Robot.FromDto(dto)); PUT methods do not return object (bug)
        }
    }
}
