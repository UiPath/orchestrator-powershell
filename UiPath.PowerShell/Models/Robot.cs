using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Models
{
    public class Robot
    {
        public long Id { get; private set; }
        public string LicenseKey { get; private set; }
        public string MachineName { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Username { get; private set; }
        public string Type { get; private set; }

        internal static Robot FromDto(RobotDto dto)
        {
            return new Robot
            {
                Id = dto.Id.Value,
                LicenseKey = dto.LicenseKey,
                MachineName = dto.MachineName,
                Name = dto.Name,
                Description = dto.Description,
                Username = dto.Username,
                Type = dto.Type.ToString()
            };
        }
    }
}
