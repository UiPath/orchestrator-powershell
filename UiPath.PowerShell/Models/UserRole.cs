using System.Collections;
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

        public Hashtable UserEntity { get; set; }

        public Hashtable[] Roles { get; set; }

        public static UserRole FromDto(UserRolesDto dto) => dto.To<UserRole>();
    }
}
