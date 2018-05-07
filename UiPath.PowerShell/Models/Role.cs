using System.Collections.Generic;
using System.Linq;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Models
{
    public class Role
    {
        public string Name { get; private set; }
        public int Id { get; private set; }
        public string DisplayName { get; private set; }
        public string Groups { get; private set; }
        public bool? IsEditable { get; private set; }
        public bool? IsStatic { get; private set; }
        public List<string> Permissions { get; private set; }

        internal static Role FromDto(RoleDto dto)
        {
            return new Role
            {
                Name = dto.Name,
                Id = dto.Id.Value,
                DisplayName = dto.DisplayName,
                Groups = dto.Groups,
                IsStatic = dto.IsStatic,
                Permissions = dto.Permissions?.Where(p => p.IsGranted.HasValue && p.IsGranted.Value && Permission.IsVisiblePermission(p)).Select(p => p.Name).ToList()
            };
        }

        internal static RoleDto ToDto(Role role)
        {
            return new RoleDto
            {
                Name = role.Name,
                Id = role.Id,
                DisplayName = role.DisplayName,
                Groups = role.Groups,
                IsStatic = role.IsStatic,
                Permissions = role.Permissions?.Select(p => new PermissionDto
                {
                    Name = p,
                    IsGranted = true,
                    RoleId = role.Id
                }).ToList()
            };
        }
    }
}
