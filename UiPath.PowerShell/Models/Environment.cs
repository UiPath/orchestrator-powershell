using System.ComponentModel;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class Environment
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Type { get; private set; }

        internal static Environment FromDto(EnvironmentDto dto)
        {
            return new Environment
            {
                Id = dto.Id.Value,
                Name = dto.Name,
                Description = dto.Description,
                Type = dto.Type.ToString(),
            };
        }
    }
}
