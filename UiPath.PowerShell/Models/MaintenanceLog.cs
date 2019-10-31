using System;
using System.ComponentModel;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class MaintenanceLog
    {
        public string State { get; internal set; }

        public DateTime? TimeStamp { get; internal set; }

        internal static MaintenanceLog FromDto(MaintenanceStateLog input)
        {
            return new MaintenanceLog()
            {
                State = input.State.ToString(),
                TimeStamp = input.TimeStamp,
            };
        }
    }
}
