using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class ProcessSchedule
    {
        public string EnvironmentName { get; internal set; }
        public bool? Enabled { get; internal set; }
        public string Name { get; internal set; }
        public string PackageName { get; internal set; }
        public string ReleaseKey { get; internal set; }
        public string ReleaseName { get; internal set; }
        public long ReleaseId { get; internal set; }
        public string StartProcessCron { get; internal set; }
        public string StopProcessCron { get; internal set; }
        public string TimeZoneIana { get; internal set; }
        public string TimeZoneId { get; internal set; }
        public string StopStrategy { get; internal set; }
        public IList<Robot> ExecutorRobots { get; internal set; }
        public string ExternalJobKey { get; internal set; }
        public long? Id { get; internal set; }
        public string StartProcessCronSummary { get; private set; }

        public long? QueueDefinitionId { get; set; }
        public string QueueDefinitionName { get; set; }
        public long? ItemsActivationThreshold { get; set; }
        public long? ItemsPerJobActivationTarget { get; set; }
        public int? MaxJobsForActivation { get; set; }

        internal static ProcessSchedule FromDto<T>(T dto) where T : new() => dto.To<ProcessSchedule>(schedule =>
        {
            var executors = dto.Extract<List<object>>("ExecutorRobots");
            schedule.ExecutorRobots = executors?.Select(re => Robot.FromDto(re)).ToList();
        });
    }
}
