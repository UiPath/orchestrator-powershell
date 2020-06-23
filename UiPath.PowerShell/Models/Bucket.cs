using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20204.Models;

namespace UiPath.PowerShell.Models
{
    [Flags]
    public enum BucketOptions
    {
        None,
        ReadOnly,
        AuditReadAccess,
    }

    [TypeConverter(typeof(UiPathTypeConverter))]

    public class Bucket
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string StorageProvider { get; private set; }
        public Guid Identifier { get; private set; }
        public BucketOptions Options { get; private set; }
        public string StorageContainer { get; private set; }
        public string StorageParameters { get; private set; }
        public long? CredentialStoreId { get; private set; }

        public static  Bucket FromDto(BucketDto dto) =>
            new Bucket
            {
                Id = dto.Id.Value,
                Name = dto.Name,
                Description = dto.Description,
                StorageProvider = dto.StorageProvider,
                Identifier = dto.Identifier,
                Options = (BucketOptions) Enum.Parse(typeof(BucketOptions), dto.Options?.ToString()),
                StorageContainer = dto.StorageContainer,
                StorageParameters = dto.StorageParameters,
                CredentialStoreId = dto.CredentialStoreId,
            };
    }
}
