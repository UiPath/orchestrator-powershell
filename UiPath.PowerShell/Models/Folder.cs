using System.ComponentModel;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class Folder
    {
        public long Id { get; set; }

        public string DisplayName { get; set; }

        public string FullyQualifiedName { get; set; }

        public string Description { get; set; }

        public FolderDtoProvisionType? ProvisionType { get; set; }

        public FolderDtoPermissionModel? PermissionModel { get; set; }

        public long? ParentId { get; set; }

        public static Folder FromDto(FolderDto dto)
        {
            return new Folder
            {
                Id = dto.Id.Value,
                DisplayName = dto.DisplayName,
                FullyQualifiedName = dto.FullyQualifiedName,
                Description = dto.Description,
                ProvisionType = dto.ProvisionType,
                PermissionModel = dto.PermissionModel,
                ParentId = dto.ParentId,
            };
        }

        public override string ToString() => FullyQualifiedName;
    }
}
