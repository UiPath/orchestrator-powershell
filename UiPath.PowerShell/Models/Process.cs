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

        internal static Process FromDto(ReleaseDto dto)
        {
            return new Process
            {
                Id = dto.Id.Value,
                IsLatestVersion = dto.IsLatestVersion,
                IsProcessDeleted = dto.IsProcessDeleted,
                Name = dto.Name,
                Description = dto.Description,
                EnvironmentId = dto.EnvironmentId,
                Key = dto.Key,
                ProcessId = dto.ProcessKey,
                ProcessVersion = dto.ProcessVersion,
            };
        }
    }
}
