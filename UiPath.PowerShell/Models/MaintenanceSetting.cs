using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UiPath.PowerShell.Util;
using MaintenanceDto = UiPath.Web.Client201910.Models.MaintenanceSetting;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class MaintenanceSetting
    {
        public string State { get; internal set; }

        public IList<string> MaintenanceLogs { get; internal set; }

        public int JobStopsAttempted { get; internal set; }

        public int JobKillsAttempted { get; internal set; }

        public int TriggersSkipped { get; internal set; }

        public int SystemTriggersSkipped { get; internal set; }

        internal static MaintenanceSetting FromDto(MaintenanceDto input)
        {
            return new MaintenanceSetting()
            {
                State = input.State.ToString(),
                MaintenanceLogs = input.MaintenanceLogs?.Select(x => JsonConvert.SerializeObject(MaintenanceLog.FromDto(x))).ToList(),
                JobStopsAttempted = input.JobStopsAttempted.Value,
                JobKillsAttempted = input.JobKillsAttempted.Value,
                TriggersSkipped = input.TriggersSkipped.Value,
                SystemTriggersSkipped = input.SystemTriggersSkipped.Value,
            };
        }
    }
}