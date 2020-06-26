using System;
using System.Collections;
using System.ComponentModel;
using UiPath.PowerShell.Util;
using MachineDto20182 = UiPath.Web.Client20182.Models.MachineDto;
using MachineDto20183 = UiPath.Web.Client20183.Models.MachineDto;
using MachineDtoType = UiPath.Web.Client20183.Models.MachineDtoType;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class Machine
    {
        public long? Id { get; private set; }
        public string LicenseKey { get; private set; }
        public string Name { get; private set; }
        public int? NonProductionSlots { get; private set; }
        public string Type { get; private set; }
        public int? UnattendedSlots { get; private set; }
        public Hashtable RobotVersions { get; private set; }
        public int? TestAutomationSlots { get; private set; }
        public int? HeadlessSlots { get; private set; }

        internal static object FromDto<TDto>(TDto dto) => dto.To<Machine>();

        internal MachineDto20182 ToDto20182(Machine machine) => new MachineDto20182
        {
            Id = Id,
            LicenseKey = LicenseKey,
            Name = Name,
            NonProductionSlots = NonProductionSlots,
            UnattendedSlots = UnattendedSlots
        };

        internal MachineDto20183 ToDto20183(Machine machine) => new MachineDto20183
        {
            Id = Id,
            LicenseKey = LicenseKey,
            Name = Name,
            NonProductionSlots = NonProductionSlots,
            Type = (MachineDtoType)Enum.Parse(typeof(MachineDtoType), Type),
            UnattendedSlots = UnattendedSlots
        };
    }
}