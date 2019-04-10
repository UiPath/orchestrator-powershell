using System;
using System.ComponentModel;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
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
