using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Models
{
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
        public UserDtoType? Type { get; private set; }
        public string UserName { get; private set; }
        public IEnumerable<int?> UserRoles { get; private set; }

        internal static User FromDto(UserDto dto)
        {
            return new User
            {
                AuthenticationSource = dto.AuthenticationSource,
                CreationTime = dto.CreationTime,
                EmailAddress = dto.EmailAddress,
                FullName = dto.FullName,
                Id = dto.Id.Value,
                IsActive = dto.IsActive,
                IsEmailConfirmed = dto.IsEmailConfirmed,
                LastLoginTime = dto.LastLoginTime,
                LoginProviders = dto.LoginProviders,
                Name = dto.Name,
                RolesList = dto.RolesList,
                Surname = dto.Surname,
                TenancyName = dto.TenancyName,
                Type = dto.Type,
                UserName = dto.UserName,
                UserRoles = dto.UserRoles?.Select(r => r.RoleId)
            };
        }
    }
}
