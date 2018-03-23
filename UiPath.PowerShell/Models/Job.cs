using System;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Models
{
    public class Job
    {
        public long Id { get; private set; }
        public string Info { get; private set; }
        public Guid? Key { get; private set; }
        public SimpleReleaseDto Release { get; private set; }
        public long? RobotId { get; private set; }
        public string Source { get; private set; }
        public DateTime? StartTime { get; private set; }
        public JobDtoState? State { get; private set; }
        public long? StartingScheduleId { get; private set; }
        public Guid? BatchExecutionKey { get; private set; }
        public DateTime? CreationTime { get; private set; }
        public DateTime? EndTime { get; private set; }

        internal static Job FromDto(JobDto dto)
        {
            return new Job
            {
                Id = dto.Id.Value,
                Info = dto.Info,
                Key = dto.Key,
                Release = dto.Release,
                RobotId = dto.Robot?.Id.Value,
                Source = dto.Source,
                StartTime = dto.StartTime,
                State = dto.State,
                StartingScheduleId = dto.StartingScheduleId,
                BatchExecutionKey = dto.BatchExecutionKey,
                CreationTime = dto.CreationTime,
                EndTime = dto.EndTime
            };
        }
    }
}
