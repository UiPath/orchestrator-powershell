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
        public MachineDtoType? Type { get; private set; }
        public int? UnattendedSlots { get; private set; }

        internal static object FromDto(MachineDto20182 dto)
        {
            return new Machine
            {
                Id = dto.Id,
                LicenseKey = dto.LicenseKey,
                Name = dto.Name,
                NonProductionSlots = dto.NonProductionSlots,
                UnattendedSlots = dto.UnattendedSlots
            };
        }

        internal MachineDto20182 ToDto20182(Machine machine)
        {
            return new MachineDto20182
            {
                Id = Id,
                LicenseKey = LicenseKey,
                Name = Name,
                NonProductionSlots = NonProductionSlots,
                UnattendedSlots = UnattendedSlots
            };
        }

        internal MachineDto20183 ToDto20183(Machine machine)
        {
            return new MachineDto20183
            {
                Id = Id,
                LicenseKey = LicenseKey,
                Name = Name,
                NonProductionSlots = NonProductionSlots,
                Type = Type,
                UnattendedSlots = UnattendedSlots
            };
        }


        internal static object FromDto(MachineDto20183 dto)
        {
            return new Machine
            {
                Id = dto.Id,
                LicenseKey = dto.LicenseKey,
                Name = dto.Name,
                NonProductionSlots = dto.NonProductionSlots,
                UnattendedSlots = dto.UnattendedSlots,
                Type = dto.Type
            };
        }
    }
}