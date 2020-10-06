using System.ComponentModel;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20204.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class MachineFolder
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int? NonProductionSlots { get; set; }
        public int? UnattendedSlots { get; set; }
        public int? HeadlessSlots { get; set; }
        public int? TestAutomationSlots { get; set; }
        public Folder Folder { get; private set; }

        public static MachineFolder FromDto(MachineFolderDto dto) => dto.To<MachineFolder>();

        internal MachineFolder WithFolder(Folder folder)
        {
            Folder = folder;
            return this;
        }
    }
}
