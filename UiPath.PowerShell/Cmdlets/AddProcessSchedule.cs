using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using UiPath.Web.Client20194.Models;
using ProcessSchedule20194Dto = UiPath.Web.Client20194.Models.ProcessScheduleDto;
using ProcessSchedule201910Dto = UiPath.Web.Client201910.Models.ProcessScheduleDto;

namespace UiPath.PowerShell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Adds a process schedule into Orchestrator</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Add, Nouns.ProcessSchedule)]
    public class AddProcessSchedule : AuthenticatedCmdlet
    {
        private const string QueueSet = "QueueSet";
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

        [Parameter(Mandatory = false, ParameterSetName = AllRobotsSet)]
        [Parameter(Mandatory = false, ParameterSetName = RobotCountSet)]
        [Parameter(Mandatory = false, ParameterSetName = SpecificRobotsSet)]
        public string TimeZoneId { get; private set; } = "UTC";

        [Parameter(Mandatory = false, ParameterSetName = AllRobotsSet)]
        [Parameter(Mandatory = false, ParameterSetName = RobotCountSet)]
        [Parameter(Mandatory = false, ParameterSetName = SpecificRobotsSet)]
        public int? StopAfterMinutes { get; private set; }

        [ValidateEnum(typeof(ProcessScheduleDtoStopStrategy))]
        [Parameter(Mandatory = false, ParameterSetName = AllRobotsSet)]
        [Parameter(Mandatory = false, ParameterSetName = RobotCountSet)]
        [Parameter(Mandatory = false, ParameterSetName = SpecificRobotsSet)]
        public string StopStrategy { get; private set; } = ProcessScheduleDtoStopStrategy.Kill.ToString();

        [Parameter(Mandatory = true, ParameterSetName = QueueSet)]
        public QueueDefinition Queue { get; private set; }

        [Parameter(Mandatory = false, ParameterSetName = QueueSet)]
        public long? ItemsActivationThreshold { get; private set; }

        [Parameter(Mandatory = false, ParameterSetName = QueueSet)]
        public long? ItemsPerJobActivationTarget { get; private set; }


        [Parameter(Mandatory = false, ParameterSetName = QueueSet)]
        public int? MaxJobsForActivation { get; private set; }

        protected override void ProcessRecord()
        {
            if (ParameterSetName == AllRobotsSet ||
                ParameterSetName == SpecificRobotsSet ||
                ParameterSetName == RobotCountSet)
            {
                AddClassicCronSchedule();
            }
            else if (ParameterSetName == QueueSet)
            {
                AddQueueSchedule();
            }
        }

        private void AddQueueSchedule()
        {
            var dto = new ProcessSchedule201910Dto
            {
                Name = Name,
                QueueDefinitionId = Queue.Id,
                ReleaseId = Process.Id,
                ItemsActivationThreshold = ItemsActivationThreshold,
                ItemsPerJobActivationTarget = ItemsPerJobActivationTarget,
                MaxJobsForActivation = MaxJobsForActivation,
                TimeZoneId = TimeZoneId,
                StartProcessCron = StartProcessCron,
                StartProcessCronDetails = "{\"type\": 5, \"advancedCronExpression\":\"" + StartProcessCron + "\"}",
                StartStrategy = -1,
            };

            var schedule = HandleHttpResponseException(() => Api_19_10.ProcessSchedules.PostWithHttpMessagesAsync(dto));
            WriteObject(ProcessSchedule.FromDto(schedule));
        }

        private void AddClassicCronSchedule()
        {
            var dto = new ProcessSchedule20194Dto
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
