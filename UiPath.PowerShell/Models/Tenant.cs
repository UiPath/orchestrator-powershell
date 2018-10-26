using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Models
{
    public class Tenant
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string AdminName { get; private set; }
        public string AdminSurname { get; private set; }
        public string AdminEmailAddress { get; private set; }
        public bool? IsActive { get; private set; }
        public DateTime? LastLoginTime { get; private set; }

        internal static Tenant ForDto(TenantDto dto)
        {
            return new Tenant
            {
                Id = dto.Id.Value,
                Name = dto.Name,
                AdminName = dto.AdminName,
                AdminSurname = dto.AdminSurname,
                AdminEmailAddress = dto.AdminEmailAddress,
                IsActive = dto.IsActive,
                LastLoginTime= dto.LastLoginTime
            };
        }
    }
}
