using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Robot)]
    public class GetRobot:FilteredCmdlet
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

        protected override void ProcessRecord()
        {
            ProcessImpl(
                filter => Api.Robots.GetRobots(filter: filter).Value, 
                id => Api.Robots.GetById(id), 
                dto => Robot.FromDto(dto));
        }
    }
}
