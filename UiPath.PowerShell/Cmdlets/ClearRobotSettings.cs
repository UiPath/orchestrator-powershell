using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Clear, Nouns.RobotSettings)]
    public class ClearRobotSettings: AuthenticatedCmdlet
    {
        private const string IdParameterSet = "Id";
        
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = IdParameterSet)]
        public long Id { get; set; }

        [SetParameter]
        [Parameter(Mandatory = false)]
        public bool TracingLevel { get; set; }

        [SetParameter]
        [Parameter(Mandatory = false)]
        public bool LoginToConsole { get; set; }

        [SetParameter]
        [Parameter(Mandatory = false)]
        public bool ResolutionWidth { get; set; }

        [SetParameter]
        [Parameter(Mandatory = false)]
        public bool ResolutionHeight { get; set; }

        [SetParameter]
        [Parameter(Mandatory = false)]
        public bool ResolutionDepth { get; set; }

        [SetParameter]
        [Parameter(Mandatory = false)]
        public bool FontSmoothing { get; set; }

        [SetParameter]
        [Parameter(Mandatory = false)]
        public bool StudioNotifyServer { get; set; }

        protected override void ProcessRecord()
        {
            var robot = HandleHttpOperationException(() => Api.Robots.GetById(Id));

            foreach (var p in this.GetType()
                .GetProperties()
                .Where(pi => pi.GetCustomAttributes(true)
                    .Where(o => o is SetParameterAttribute)
                    .Any()))
            {
                var shouldClear = p.GetValue(this);
                if (shouldClear != null && (bool)shouldClear && robot.ExecutionSettings.ContainsKey(p.Name))
                {
                    robot.ExecutionSettings.Remove(p.Name);
                }
            }

            if (!robot.ExecutionSettings.Any())
            {
                robot.ExecutionSettings = null;
            }

            HandleHttpOperationException(() => Api.Robots.PutById(robot.Id.Value, robot));
        }
    }
}
