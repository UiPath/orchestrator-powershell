using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.Edit, Nouns.Robot)]
    public class EditRobot : EditCmdlet
    {

        private const string RobotParameterSet = "Robot";
        private const string IdParameterSet = "Id";

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = RobotParameterSet)]
        public Robot Robot { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = IdParameterSet)]
        public long Id { get; set; }

        [SetParameter]
        [Parameter]
        public string Name { get; private set; }

        [SetParameter]
        [Parameter]
        public string MachineName { get; private set; }

        [SetParameter]
        [Parameter]
        public string LicenseKey { get; private set; }

        [SetParameter]
        [Parameter]
        public string Username { get; private set; }

        //[Credential]
        [SetParameter]
        [Parameter]
        public string Password { get; private set; }

        [SetParameter]
        [Parameter]
        public string Description { get; private set; }

        [SetParameter]
        [ValidateEnum(typeof(RobotDtoType))]
        [Parameter]
        public string Type { get; set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                () => (ParameterSetName == RobotParameterSet) ? Robot.ToDto20181(Robot) : HandleHttpOperationException(() => Api.Robots.GetById(Id)),
                (robotDto) => HandleHttpOperationException(() => Api.Robots.PutById(robotDto.Id.Value, robotDto)));
        }
    }
}
