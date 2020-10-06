using System.ComponentModel;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class ExtendedFolder: Folder
    {
        public bool? IsSelectable { get; set; }

        public bool? HasChildren { get; set; }

        public int? Level { get; set; }

        public static ExtendedFolder FromDto(ExtendedFolderDto dto) => dto.To<ExtendedFolder>();
    }
}
