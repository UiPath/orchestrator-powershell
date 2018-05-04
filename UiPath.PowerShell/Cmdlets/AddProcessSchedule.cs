using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Adds a process schedule into Orchestrator</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Add, Nouns.ProcessSchedule)]
    public class AddProcessSchedule : AuthenticatedCmdlet
    {
        private const string AllRobotsSet = "AllRobots";
        private const string RobotCountSet = "RobotCount";
        private const string SpecificRobotsSet = "SpecificRobots";


        [Parameter(Mandatory = true, Position = 0)]
        public string Name { get; private set; }

        [Parameter(Mandatory = true)]
        public string StartProcessCron { get; private set; }

        [Parameter(Mandatory = true)]
        public Process Process { get; private set; }

        [Parameter(Mandatory = true, ParameterSetName = AllRobotsSet)]
        public SwitchParameter AllRobots { get; private set; }

        [Parameter(Mandatory = true, ParameterSetName = RobotCountSet)]
        public int? RobotCount { get; private set; }


        [Parameter(Mandatory = true, ParameterSetName = SpecificRobotsSet)]
        public List<Robot> Robots { get; private set; }

        [Parameter]
        public string TimeZoneId { get; private set; } = "UTC";

        [Parameter]
        public int? StopAfterMinutes { get; private set; }

        [ValidateEnum(typeof(ProcessScheduleDtoStopStrategy))]
        [Parameter]
        public string StopStrategy { get; private set; } = ProcessScheduleDtoStopStrategy.Kill.ToString();

        protected override void ProcessRecord()
        {
            var dto = new ProcessScheduleDto
            {
                Name = Name,
                StartProcessCron = StartProcessCron,
                StartProcessCronDetails = "{\"type\": 5, \"advancedCronExpression\":\"" + StartProcessCron + "\"}",
                ReleaseId = Process.Id,
                TimeZoneId = TimeZoneId
            };

            if (StopAfterMinutes.HasValue)
            {
                dto.StopProcessExpression = TimeSpan.FromMinutes(StopAfterMinutes.Value).TotalSeconds.ToString();
                dto.StopStrategy = (ProcessScheduleDtoStopStrategy) Enum.Parse(typeof(ProcessScheduleDtoStopStrategy), StopStrategy);
            }

            if (ParameterSetName == AllRobotsSet)
            {
                dto.StartStrategy = -1;
            }
            else if (ParameterSetName == RobotCountSet)
            {
                dto.StartStrategy = RobotCount;
            }
            else if (ParameterSetName == SpecificRobotsSet)
            {
                dto.StartStrategy = 0;
                dto.ExecutorRobots = Robots.Select(r => new RobotExecutorDto
                {
                    Id = r.Id
                }).ToList();
            }

            var schedule = HandleHttpOperationException(() => Api.ProcessSchedules.Post(dto));
            WriteObject(ProcessSchedule.FromDto(schedule));
        }
    }
}
