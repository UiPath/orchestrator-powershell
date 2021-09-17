using System;
using RobotExecutorDto = UiPath.Web.Client20194.Models.RobotExecutorDto;
using RobotDtoType20194 = UiPath.Web.Client20194.Models.RobotDtoType;
using RobotDtoHostingType20194 = UiPath.Web.Client20194.Models.RobotDtoHostingType;
using RobotDto20194 = UiPath.Web.Client20194.Models.RobotDto;
using RobotDto201910 = UiPath.Web.Client201910.Models.RobotDto;
using RobotDto20204 = UiPath.Web.Client20204.Models.RobotDto;
using UiPath.PowerShell.Util;
using System.ComponentModel;
using System.Collections;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
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
        public long? CredentialStoreId { get; private set; }
        public Hashtable ExecutionSettings { get; private set; }
        public string ExternalName { get; private set; }
        public string ProvisionType { get; private set; }
        public bool? IsExternalLicensed { get; private set; }
        public long? MachineId { get; private set; }

        internal static Robot FromDto<TDto>(TDto dto) => dto.To<Robot>();

        internal static RobotDto20194 ToDto20183(Robot robot)
        {
            return new RobotDto20194
            {
                Id = robot.Id,
                LicenseKey = robot.LicenseKey,
                MachineName = robot.MachineName,
                Name = robot.Name,
                Description = robot.Description,
                Username = robot.Username,
                Type = (RobotDtoType20194)Enum.Parse(typeof(RobotDtoType20194), robot.Type),
                HostingType = (RobotDtoHostingType20194)Enum.Parse(typeof(RobotDtoHostingType20194), robot.HostingType)
            };
        }
    }
}
