using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Start, Nouns.Job)]
    public class StartJob: AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "RobotCount")]
        public int? RobotCount { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "All")]
        public SwitchParameter All { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Robots")]
        public List<long> Robots { get; set; }

        [Parameter(Mandatory = true)]
        public Process Process { get; set; }

        protected override void ProcessRecord()
        {
            var startJob = new StartJobParameters
            {
                StartInfo = new StartProcessDto
                {
                    ReleaseKey = Process.Key,
                    Source = StartProcessDtoSource.Manual,
                }
            };
            if (All.IsPresent)
            {
                startJob.StartInfo.Strategy = StartProcessDtoStrategy.All;
            }
            else if (RobotCount.HasValue)
            {
                startJob.StartInfo.Strategy = StartProcessDtoStrategy.RobotCount;
                startJob.StartInfo.NoOfRobots = RobotCount.Value;
            }
            else if (Robots != null)
            {
                startJob.StartInfo.Strategy = StartProcessDtoStrategy.Specific;
                startJob.StartInfo.RobotIds = Robots.Cast<long?>().ToList();
            }
            var jobs = HandleHttpOperationException(() => Api.Jobs.StartJobs(startJob));
            foreach(var dto in jobs.Value)
            {
                WriteObject(Models.Job.FromDto(dto));
            }
        }
    }
}
