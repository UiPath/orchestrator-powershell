using UiPath.Web.Client20183.Models;

namespace UiPath.PowerShell.Models
{
    public class Machine
    {
        public long? Id { get; private set; }
        public string LicenseKey { get; private set; }
        public string Name { get; private set; }
        public int? NonProductionSlots { get; private set; }
        public MachineDtoType? Type { get; private set; }
        public int? UnattendedSlots { get; private set; }

        internal static object FromDto(MachineDto dto)
        {
            return new Machine
            {
                Id = dto.Id,
                LicenseKey = dto.LicenseKey,
                Name = dto.Name,
                NonProductionSlots = dto.NonProductionSlots,
                Type = dto.Type,
                UnattendedSlots = dto.UnattendedSlots
            };
        }

        internal MachineDto ToDto(Machine machine)
        {
            return new MachineDto
            {
                Id = Id,
                LicenseKey = LicenseKey,
                Name = Name,
                NonProductionSlots = NonProductionSlots,
                Type = Type,
                UnattendedSlots = UnattendedSlots
            };
        }
    }
}