using System;
using System.Collections.Generic;
using System.ComponentModel;
using UiPath.PowerShell.Util;

namespace UiPath.PowerShell.Models
{

    [TypeConverter(typeof(UiPathTypeConverter))]
    public class User
    {
        public string AuthenticationSource { get; private set; }
        public DateTime? CreationTime { get; private set; }
        public string EmailAddress { get; private set; }
        public string FullName { get; private set; }
        public long Id { get; private set; }
        public bool? IsActive { get; private set; }
        public bool? IsEmailConfirmed { get; private set; }
        public DateTime? LastLoginTime { get; private set; }
        public IList<string> LoginProviders { get; private set; }
        public string Name { get; private set; }
        public IList<string> RolesList { get; private set; }
        public string Surname { get; private set; }
        public string TenancyName { get; private set; }
        public string Type { get; private set; }
        public string UserName { get; private set; }
        public IEnumerable<int?> UserRoles { get; private set; }
        public string ProvisionType { get; private set; }
        public bool? MayHavePersonalWorkspace { get; private set; }
        public bool? MayHaveRobotSession { get; private set; }
        public bool? MayHaveUserSession { get; private set; }
        public bool? MayHaveUnattendedSession { get; private set; }
        public string LicenseType { get; private set; }
        public bool? BypassBasicAuthRestriction { get; private set; }
        public string Domain { get; private set; }
        public bool? IsExternalLicensed { get; private set; }

        internal static User FromDto<TDto>(TDto dto) => dto.To<User>();
    }
}
