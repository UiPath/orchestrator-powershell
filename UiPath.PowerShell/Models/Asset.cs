using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Models
{
    /// <summary>
    /// <para type="description">An UiPath Orchestrator asset</para>
    /// </summary>
    public class Asset
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public AssetDtoValueType? ValueType { get; private set; }
        public AssetDtoValueScope ValueScope { get; private set; }
        public string Value { get; private set; }
        public Dictionary<long, string> RobotValues { get; private set; }
        public bool? CanBeDeleted { get; private set; }
        public Dictionary<string, string> KeyValueList { get; private set; }
        public PSCredential WindowsCredential { get; private set; }
        public PSCredential Credential { get; private set; }

        internal static Asset FromDto(AssetDto dto)
        {
            return new Asset
            {
                Id = dto.Id.Value,
                Name = dto.Name,
                ValueType = dto.ValueType,
                ValueScope = dto.ValueScope,
                Value = dto.Value,
                RobotValues = dto.RobotValues?.ToDictionary(r => r.RobotId.Value, r=> r.Value),
                KeyValueList = dto.KeyValueList?.ToDictionary(kv => kv.Key, kv => kv.Value),
                CanBeDeleted = dto.CanBeDeleted,
                WindowsCredential = (dto.ValueType == AssetDtoValueType.WindowsCredential) ?
                    new PSCredential(
                        dto.CredentialUsername,
                        Cmdlets.AddAsset.MakeSecureString(dto.CredentialPassword)) : null,
                Credential = (dto.ValueType == AssetDtoValueType.Credential) ?
                    new PSCredential(
                        dto.CredentialUsername,
                        Cmdlets.AddAsset.MakeSecureString(dto.CredentialPassword)) : null
            };
        }
    }
}
