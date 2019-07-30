using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiPath.PowerShell.Util;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class RobotExecutionSettings
    {
        public long Id { get; private set; }
        public string TracingLevel { get; private set; }
        public bool? LoginToConsole { get; private set; }
        public long? ResolutionWidth { get; private set; }
        public long? ResolutionHeight { get; private set; }
        public long? ResolutionDepth { get; private set; }
        public bool? FontSmoothing { get; private set; }
        public bool? StudioNotifyServer { get; private set; }

        internal static RobotExecutionSettings FromExecutionSettingsDictionary(long id, IDictionary<string, object> executionSettings)
        {
            if (executionSettings == null)
            {
                return new RobotExecutionSettings();
            }

            return new RobotExecutionSettings()
            {
                Id = id,
                TracingLevel = TryGetParameterAs<string>(executionSettings, nameof(TracingLevel)),
                LoginToConsole = TryGetStructParameterAs<bool>(executionSettings, nameof(LoginToConsole)),
                ResolutionWidth = TryGetStructParameterAs<long>(executionSettings, nameof(ResolutionWidth)),
                ResolutionHeight = TryGetStructParameterAs<long>(executionSettings, nameof(ResolutionHeight)),
                ResolutionDepth = TryGetStructParameterAs<long>(executionSettings, nameof(ResolutionDepth)),
                FontSmoothing = TryGetStructParameterAs<bool>(executionSettings, nameof(FontSmoothing)),
                StudioNotifyServer = TryGetStructParameterAs<bool>(executionSettings, nameof(StudioNotifyServer))
            };
        }

        internal static IDictionary<string, object> ToDictionary(RobotExecutionSettings settings)
        {
            var dict = new Dictionary<string, object>();
            foreach (var p in typeof(RobotExecutionSettings).GetProperties())
            {
                if (p.Name == nameof(Id)) continue;

                var value = p.GetValue(settings);
                if (value != null)
                {
                    dict[p.Name] = value;
                }
            }

            return dict;
        }

        private static T TryGetParameterAs<T>(IDictionary<string, object> executionSettings, string name) where T : class
        {
            if (!executionSettings.ContainsKey(name))
            {
                return null;
            }

            return executionSettings[name] as T;
        }

        private static T? TryGetStructParameterAs<T>(IDictionary<string, object> executionSettings, string name) where T : struct
        {
            if (!executionSettings.ContainsKey(name))
            {
                return null;
            }

            var p = executionSettings[name];
            return p as T?;
        }
    }
}
