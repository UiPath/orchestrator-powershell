using System;
using RobotExecutorDto = UiPath.Web.Client20181.Models.RobotExecutorDto;
using RobotDtoType20181 = UiPath.Web.Client20181.Models.RobotDtoType;
using RobotDtoType20183 = UiPath.Web.Client20183.Models.RobotDtoType;
using RobotDtoHostingType20183 = UiPath.Web.Client20183.Models.RobotDtoHostingType;
using RobotDto20181 = UiPath.Web.Client20181.Models.RobotDto;
using RobotDto20183 = UiPath.Web.Client20183.Models.RobotDto;
using RobotDto20184 = UiPath.Web.Client20184.Models.RobotDto;

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
        public string HostingType { get; private set; }
        public string CredentialType { get; private set; }

        internal static Robot FromDto(RobotDto20181 dto)
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

        internal static RobotDto20181 ToDto20181(Robot robot)
        {
            return new RobotDto20181
            {
                Id = robot.Id,
                LicenseKey = robot.LicenseKey,
                MachineName = robot.MachineName,
                Name = robot.Name,
                Description = robot.Description,
                Username = robot.Username,
                Type = (RobotDtoType20181)Enum.Parse(typeof(RobotDtoType20181), robot.Type)
            };
        }

        internal static RobotDto20183 ToDto20183(Robot robot)
        {
            return new RobotDto20183
            {
                Id = robot.Id,
                LicenseKey = robot.LicenseKey,
                MachineName = robot.MachineName,
                Name = robot.Name,
                Description = robot.Description,
                Username = robot.Username,
                Type = (RobotDtoType20183)Enum.Parse(typeof(RobotDtoType20183), robot.Type),
                HostingType = (RobotDtoHostingType20183)Enum.Parse(typeof(RobotDtoHostingType20183), robot.HostingType)
            };
        }

        internal static object FromDto(RobotDto20183 dto)
        {
            return new Robot
            {
                Id = dto.Id.Value,
                LicenseKey = dto.LicenseKey,
                MachineName = dto.MachineName,
                Name = dto.Name,
                Description = dto.Description,
                Username = dto.Username,
                Type = dto.Type.ToString(),
                HostingType = dto.HostingType.ToString(),
            };
        }

        internal static object FromDto(RobotDto20184 dto)
        {
            return new Robot
            {
                Id = dto.Id.Value,
                LicenseKey = dto.LicenseKey,
                MachineName = dto.MachineName,
                Name = dto.Name,
                Description = dto.Description,
                Username = dto.Username,
                Type = dto.Type.ToString(),
                HostingType = dto.HostingType.ToString(),
                CredentialType = dto.CredentialType.ToString()
            };
        }
    }
}
