using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class UserRole
    {
        public long Id { get; set; }

        public SimpleUserEntityDto UserEntity { get; set; }

        public IEnumerable<SimpleRoleDto> Roles { get; set; }

        public static UserRole FromDto(UserRolesDto dto)
        {
            return new UserRole
            {
                Id = dto.Id.Value,
                UserEntity = dto.UserEntity,
                Roles = dto.Roles.ToList()
            };
        }
    }
}
