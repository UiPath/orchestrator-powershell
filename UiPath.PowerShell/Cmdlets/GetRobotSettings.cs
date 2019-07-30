using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20181.Models;
using UiPath.Web.Client20183;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.RobotSettings)]
    public class GetRobotSettings: FilteredIdCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Name { get; set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string MachineName { get; set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string LicenseKey { get; set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Username { get; set; }

        [ValidateEnum(typeof(Web.Client20181.Models.RobotDtoType))]
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Type { get; private set; }

        [ValidateEnum(typeof(Web.Client20183.Models.RobotDtoHostingType))]
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string HostingType { get; set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                filter => Api.Robots.GetRobots(filter: filter).Value.Select(r => RobotExecutionSettings.FromExecutionSettingsDictionary(r.Id.Value, r.ExecutionSettings)).ToList(),
                id => RobotExecutionSettings.FromExecutionSettingsDictionary(id, Api.Robots.GetById(id)?.ExecutionSettings),
                dto => dto);
        }
    }
}
