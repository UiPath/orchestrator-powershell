using System;
using License20181Dto = UiPath.Web.Client20181.Models.LicenseDto;
using License20183Dto = UiPath.Web.Client20183.Models.LicenseDto;

namespace UiPath.PowerShell.Models
{
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

        internal static License FromDto(License20181Dto dto)
        {
            return new License
            {
                AllowedAttended = dto.AllowedRobots?.Attended,
                AllowedDevelopment = dto.AllowedRobots?.Development,
                AllowedNonProduction = dto.AllowedRobots?.NonProduction,
                AllowedUnattended = dto.AllowedRobots?.Unattended,

                DefinedAttended = dto.DefinedRobots?.Attended,
                DefinedDevelopment = dto.DefinedRobots?.Development,
                DefinedNonProduction = dto.DefinedRobots?.NonProduction,
                DefinedUnattended = dto.DefinedRobots?.Unattended,

                ConcurrentAttended = dto.ConcurrentRobots?.Attended,
                ConcurrentDevelopment = dto.ConcurrentRobots?.Development,
                ConcurrentNonProduction = dto.ConcurrentRobots?.NonProduction,
                ConcurrentUnattended = dto.ConcurrentRobots?.Unattended,

                Concurrent = dto.Concurrent,

                ExpireDate = UnixTimeStampToDateTime(dto.ExpireDate ?? 0),
                IsExpired = dto.IsExpired,
                IsRegistered = dto.IsRegistered,
            };
        }

        internal static License FromDto(License20183Dto dto)
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
