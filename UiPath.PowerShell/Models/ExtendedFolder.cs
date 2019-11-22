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

        public static ExtendedFolder FromDto(ExtendedFolderDto dto)
        {
            return new ExtendedFolder
            {
                Id = dto.Id.Value,
                DisplayName = dto.DisplayName,
                FullyQualifiedName = dto.FullyQualifiedName,
                Description = dto.Description,
                ProvisionType = dto.ProvisionType == ExtendedFolderDtoProvisionType.Automatic ? FolderDtoProvisionType.Automatic : FolderDtoProvisionType.Manual,
                PermissionModel = dto.PermissionModel == ExtendedFolderDtoPermissionModel.FineGrained ? FolderDtoPermissionModel.FineGrained : FolderDtoPermissionModel.InheritFromTenant,
                ParentId = dto.ParentId,
                IsSelectable = dto.IsSelectable,
                HasChildren = dto.HasChildren,
                Level = dto.Level,
            };
        }

    }
}
