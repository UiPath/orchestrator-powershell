using System;
using System.ComponentModel;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class License
    {
        public long? Id { get; private set; }
        public long? HostLicenseId { get; private set; }
        public string Code { get; private set; }
        public DateTime? CreationTime { get; private set; }


        public DateTime ExpireDate { get; private set; }
        public bool? IsExpired { get; private set; }
        public bool? IsRegistered { get; private set; }
        public long? AllowedAttended { get; private set; }
        public long? AllowedDevelopment { get; private set; }
        public long? AllowedNonProduction { get; private set; }
        public long? AllowedUnattended { get; private set; }
        public long? UsedAttended { get; private set; }
        public long? UsedDevelopment { get; private set; }
        public long? UsedNonProduction { get; private set; }
        public long? UsedUnattended { get; private set; }
        public bool? Concurrent { get; private set; }
        public bool? AttendedConcurrent { get; private set; }
        public bool? DevelopmentConcurrent { get; private set; }


        // Old style 18.1 fields
        public long? DefinedAttended { get; private set; }
        public long? DefinedDevelopment { get; private set; }
        public long? DefinedNonProduction { get; private set; }
        public long? DefinedUnattended { get; private set; }
        public long? ConcurrentAttended { get; private set; }
        public long? ConcurrentDevelopment { get; private set; }
        public long? ConcurrentNonProduction { get; private set; }
        public long? ConcurrentUnattended { get; private set; }


        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        internal static License FromDto(LicenseDto dto)
        {
            return new License
            {
                AllowedAttended = dto.Allowed?.Attended,
                AllowedDevelopment = dto.Allowed?.Development,
                AllowedNonProduction = dto.Allowed?.NonProduction,
                AllowedUnattended = dto.Allowed?.Unattended,

                UsedAttended = dto.Used?.Attended,
                UsedDevelopment = dto.Used?.Development,
                UsedNonProduction = dto.Used?.NonProduction,
                UsedUnattended = dto.Used?.Unattended,

                AttendedConcurrent = dto.AttendedConcurrent,
                DevelopmentConcurrent = dto.DevelopmentConcurrent,

                Id = dto.Id,
                HostLicenseId = dto.HostLicenseId,
                Code = dto.Code,
                CreationTime =dto.CreationTime,

                ExpireDate = UnixTimeStampToDateTime(dto.ExpireDate ?? 0),
                IsExpired = dto.IsExpired,
                IsRegistered = dto.IsRegistered,
            };
        }
    }
}
