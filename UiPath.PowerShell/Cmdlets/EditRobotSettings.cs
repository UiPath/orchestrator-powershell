using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.Edit, Nouns.RobotSettings)]
    public class EditRobotSettings: EditCmdlet
    {
        private const string RobotSettingsParameterSet = "RobotSettings";
        private const string IdParameterSet = "Id";

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = RobotSettingsParameterSet)]
        public RobotExecutionSettings RobotSettings { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = IdParameterSet)]
        public long Id { get; set; }

        [SetParameter]
        [Parameter]
        public string TracingLevel { get; set; }

        [SetParameter]
        [Parameter]
        public bool? LoginToConsole { get; set; }

        [SetParameter]
        [Parameter]
        public long? ResolutionWidth { get; set; }

        [SetParameter]
        [Parameter]
        public long? ResolutionHeight { get; set; }

        [SetParameter]
        [Parameter]
        public long? ResolutionDepth { get; set; }

        [SetParameter]
        [Parameter]
        public bool? FontSmoothing { get; set; }

        [SetParameter]
        [Parameter]
        public bool? StudioNotifyServer { get; set; }

        protected override void ProcessRecord()
        {
            RobotDto robot = null;

            ProcessImpl(() =>
            {
                robot = HandleHttpOperationException(() => Api.Robots.GetById(RobotSettings?.Id ?? Id));
                if (robot.ExecutionSettings == null)
                {
                    robot.ExecutionSettings = new Dictionary<string, object>();
                }

                return RobotExecutionSettings.FromExecutionSettingsDictionary(RobotSettings?.Id ?? Id, robot.ExecutionSettings);
            }, robotSettings =>
            {
                robot.ExecutionSettings = RobotExecutionSettings.ToDictionary(robotSettings);
                HandleHttpOperationException(() => Api.Robots.PutById(robotSettings.Id, robot));
            });
        }
    }
}
