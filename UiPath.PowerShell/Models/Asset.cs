using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Models
{
    /// <summary>
    /// <para type="description">An UiPath Orchestrator asset</para>
    /// </summary>
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class Asset
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string ValueType { get; private set; }
        public string ValueScope { get; private set; }
        public string Value { get; private set; }
        public Dictionary<long, string> RobotValues { get; private set; }
        public bool? CanBeDeleted { get; private set; }
        public Dictionary<string, string> KeyValueList { get; private set; }
        public PSCredential WindowsCredential { get; private set; }
        public PSCredential Credential { get; private set; }

        internal static Asset FromDto(AssetDto dto)
        {
            var asset = new Asset
            {
                Id = dto.Id.Value,
                Name = dto.Name,
                ValueType = dto.ValueType.ToString(),
                ValueScope = dto.ValueScope.ToString(),
                Value = dto.ValueScope == AssetDtoValueScope.Global ? dto.Value : null,
                RobotValues = dto.RobotValues?.ToDictionary(r => r.RobotId.Value, r => r.Value),
                KeyValueList = dto.KeyValueList?.ToDictionary(kv => kv.Key, kv => kv.Value),
                CanBeDeleted = dto.CanBeDeleted,
            };

            if (dto.ValueScope == AssetDtoValueScope.Global && dto.ValueType == AssetDtoValueType.WindowsCredential)
            {
                asset.WindowsCredential = new PSCredential(
                         dto.CredentialUsername,
                         Cmdlets.AddAsset.MakeSecureString(dto.CredentialPassword));
            }
            else if (dto.ValueScope == AssetDtoValueScope.Global && dto.ValueType == AssetDtoValueType.Credential)
            {
                asset.Credential = new PSCredential(
                        dto.CredentialUsername,
                        Cmdlets.AddAsset.MakeSecureString(dto.CredentialPassword));

            };
            return asset;
        }
    }
}
