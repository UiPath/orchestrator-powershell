using System.ComponentModel;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class CredentialStore
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string AdditionalConfiguration { get; private set; }
        public bool? IsReadOnly { get; private set; }

        internal static CredentialStore FromDto(CredentialStoreDto dto) =>
            new CredentialStore
            {
                Id = dto.Id.Value,
                Name = dto.Name,
                Type = dto.Type,
                AdditionalConfiguration = dto.AdditionalConfiguration,
                IsReadOnly = dto.Details?.IsReadOnly
            };
    }
}
