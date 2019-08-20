using System;
using UiPath.Web.Client201910.Models;

namespace UiPath.PowerShell.Util
{
    /// <summary>
    /// Converts maintenance logs into a dynamic objects
    /// </summary>
    public static class MaintenanceSettingConverter
    {
        private static dynamic resultTyped = new
        {
            State = "",
            MaintenanceLogs = new[] { new { State = "", TimeStamp = new Nullable<DateTime>(DateTime.Now) } },
            JobStopsAttempted = 0,
            JobKillsAttempted = 0,
            TriggersSkipped = 0,
            SystemTriggersSkipped = 0
        };

        public static dynamic ToDynamic(this MaintenanceSetting input)
        {
            var inputJson = Newtonsoft.Json.JsonConvert.SerializeObject(input);
            return Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(inputJson, resultTyped);
        }
    }
}
