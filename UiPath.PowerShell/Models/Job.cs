using System;
using System.Collections;
using System.ComponentModel;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class Job
    {
        public long Id { get; private set; }
        public string Info { get; private set; }
        public Guid? Key { get; private set; }
        public Hashtable Release { get; private set; }
        public string ReleaseName { get; private set; }
        public long? RobotId { get; private set; }
        public string Source { get; private set; }
        public DateTime? StartTime { get; private set; }
        public string State { get; private set; }
        public long? StartingScheduleId { get; private set; }
        public Guid? BatchExecutionKey { get; private set; }
        public DateTime? CreationTime { get; private set; }
        public DateTime? EndTime { get; private set; }

        public string InputArguments { get; private set; }

        public string OutputArguments { get; private set; }

        internal static Job FromDto<T>(T dto) => dto.To<Job>();
    }
}
