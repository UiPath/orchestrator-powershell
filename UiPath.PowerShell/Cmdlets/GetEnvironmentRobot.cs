using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using Environment = UiPath.PowerShell.Models.Environment;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.EnvironmentRobot)]
    public class GetEnvironmentRobot : AuthenticatedCmdlet
    {
        protected const string EnvironmentParameterSet = "EnvironmentParameterSet";
        protected const string EnvironmentIdParameterSet = "EnvironmentIdParameterSet";

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = EnvironmentParameterSet)]
        public Environment Environment { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = EnvironmentIdParameterSet)]
        public long? EnvironmentId { get; set; }

        protected override void ProcessRecord()
        {
            // GetRobotsForEnvironmentByKey gets *all* robots and sort the environment ones first
            // GetRobotIdsForEnvironmentByKey gets only the relevant robots, but returns only the Ids. 
            // Pick your poison...

            var robots = HandleHttpOperationException(() => Api.Environments.GetRobotsForEnvironmentByKey(Environment?.Id ?? EnvironmentId.Value)).Value;
            var robotIds = HandleHttpOperationException(() => Api.Environments.GetRobotIdsForEnvironmentByKey(Environment?.Id ?? EnvironmentId.Value)).Value;


            foreach(var robot in robots.Join(robotIds, r => r.Id.Value, id => id.Value, (r,id) => r))
            {
                WriteObject(Robot.FromDto(robot));
            }
        }
    }
}
