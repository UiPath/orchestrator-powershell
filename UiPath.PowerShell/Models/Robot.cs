using System;
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

        internal static Robot FromDto(RobotExecutorDto dto)
        {
            return new Robot
            {
                Id = dto.Id.Value,
                MachineName = dto.MachineName,
                Name = dto.Name,
                Description = dto.Description,
            };
        }

        internal static RobotDto ToDto(Robot robot)
        {
            return new RobotDto
            {
                Id = robot.Id,
                LicenseKey = robot.LicenseKey,
                MachineName = robot.MachineName,
                Name = robot.Name,
                Description = robot.Description,
                Username = robot.Username,
                Type = (RobotDtoType)Enum.Parse(typeof(RobotDtoType), robot.Type)
            };
        }
    }
}
