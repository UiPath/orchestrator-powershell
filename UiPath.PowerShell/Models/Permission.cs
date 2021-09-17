using System.ComponentModel;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class Permission
    {
        public long? Id { get; private set; }
        public string Name { get; private set; }
        public int? RoleId { get; private set; }
        public bool? IsGranted { get; private set; }

        internal static Permission FromDto(PermissionDto dto)
        {
            return new Permission
            {
                Id = dto.Id,
                Name = dto.Name,
                RoleId = dto.RoleId,
                IsGranted = dto.IsGranted
            };
        }

        internal static bool IsVisiblePermission(PermissionDto p)
        {
            return p.Name.Contains(".");
        }
    }
}
