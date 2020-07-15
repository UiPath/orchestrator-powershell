using System.Collections;
using System.ComponentModel;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class Process
    {
        public long Id { get; private set; }
        public bool? IsLatestVersion { get; private set; }
        public bool? IsProcessDeleted { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public long EnvironmentId { get; private set; }
        public string Key { get; private set; }
        public string ProcessId { get; private set; }
        public string ProcessVersion { get; private set; }

        public Hashtable Arguments { get; private set; }

        internal static Process FromDto<T>(T dto) => dto.To<Process>();
    }
}
