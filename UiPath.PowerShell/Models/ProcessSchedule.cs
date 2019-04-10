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
        private string startProcessCronDetails;

        public string EnvironmentName { get; internal set; }
        public bool? Enabled { get; internal set; }
        public string Name { get; internal set; }
        public string PackageName { get; internal set; }
        public string ReleaseKey { get; internal set; }
        public string ReleaseName { get; internal set; }
        public string StartProcessCron { get; internal set; }
        public string StopProcessCron { get; internal set; }
        public string TimeZoneIana { get; internal set; }
        public string TimeZoneId { get; internal set; }
        public string StopStrategy { get; internal set; }
        public IList<Robot> ExecutorRobots { get; internal set; }
        public string ExternalJobKey { get; internal set; }
        public long? Id { get; internal set; }
        public string StartProcessCronSummary { get; private set; }

        internal static ProcessSchedule FromDto(ProcessScheduleDto dto)
        {
            return new ProcessSchedule
            {
                Id = dto.Id,
                EnvironmentName = dto.EnvironmentName,
                Enabled = dto.Enabled,
                Name = dto.Name,
                PackageName = dto.PackageName,
                ReleaseKey = dto.ReleaseKey,
                ReleaseName = dto.ReleaseName,
                StartProcessCron = dto.StartProcessCron,
                startProcessCronDetails = dto.StartProcessCronDetails,
                StartProcessCronSummary = dto.StartProcessCronSummary,
                StopProcessCron = dto.StopProcessExpression,
                TimeZoneIana = dto.TimeZoneIana,
                TimeZoneId = dto.TimeZoneId,
                StopStrategy = dto.StopStrategy.ToString(),
                ExecutorRobots = dto.ExecutorRobots?.Select(re => Robot.FromDto(re)).ToList(),
                ExternalJobKey = dto.ExternalJobKey
            };
        }
    }
}
