using System.Collections;
using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Modifies an existing asset</para>
    /// <para type="description">You cannot change the asset name, type or scope. Use New-UiPathAssetRobotValue to build robot values for -AddRobotValues</para>
    /// <example>
    /// <code>Get-UiPathAsset -Name &lt;myasset&gt; -TextValue &lt;newvalue&gt;</code>
    /// <para>Modifies a Text type asset value</para>
    /// </example>
    /// </summary>
    [Cmdlet(VerbsData.Edit, Nouns.Asset)]
    public class EditAsset : AuthenticatedCmdlet
    {

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public Asset Asset { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewAssetRobotValue.TextValueSet)]
        public string TextValue { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewAssetRobotValue.IntValueSet)]
        public int? IntValue { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewAssetRobotValue.DBConnectionStringSet)]
        public string DBConnectionString { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewAssetRobotValue.HttpConnectionStringSet)]
        public string HttpConnectionString { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewAssetRobotValue.BoolValueSet)]
        public bool? BoolValue { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewAssetRobotValue.KeyValueListSet)]
        public Hashtable KeyValueList { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewAssetRobotValue.WindowsCredentialSet)]
        public PSCredential WindowsCredential { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewAssetRobotValue.CredentialSet)]
        public PSCredential Credential { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = AddAsset.RobotValuesSet)]
        public AssetRobotValue[] AddRobotValues { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = AddAsset.RobotValuesSet)]
        public long[] RemoveRobotIdValues { get; set; }

        protected override void ProcessRecord()
        {
            var dto = HandleHttpOperationException(() => Api.Assets.GetAssets(filter: $"Name eq '{Asset.Name}'", expand: "RobotValues").Value.First(a => a.Name == Asset.Name));
            ProcessDto(dto);
        }

        private void ProcessDto(AssetDto dto)
        {
            if (dto.ValueScope == AssetDtoValueScope.Global && ParameterSetName != AddAsset.RobotValuesSet)
            {
                switch (ParameterSetName)
                {
                    case NewAssetRobotValue.TextValueSet:
                        dto.ValueType = AssetDtoValueType.Text;
                        dto.StringValue = TextValue;
                        break;
                    case NewAssetRobotValue.IntValueSet:
                        dto.ValueType = AssetDtoValueType.Integer;
                        dto.IntValue = IntValue;
                        break;
                    case NewAssetRobotValue.BoolValueSet:
                        dto.ValueType = AssetDtoValueType.Bool;
                        dto.BoolValue = BoolValue;
                        break;
                    case NewAssetRobotValue.DBConnectionStringSet:
                        dto.ValueType = AssetDtoValueType.DBConnectionString;
                        dto.StringValue = DBConnectionString;
                        break;
                    case NewAssetRobotValue.HttpConnectionStringSet:
                        dto.ValueType = AssetDtoValueType.HttpConnectionString;
                        dto.StringValue = HttpConnectionString;
                        break;
                    case NewAssetRobotValue.KeyValueListSet:
                        dto.ValueType = AssetDtoValueType.KeyValueList;
                        dto.KeyValueList = KeyValueList.ToKeyList();
                        break;
                    case NewAssetRobotValue.WindowsCredentialSet:
                        dto.ValueType = AssetDtoValueType.WindowsCredential;
                        dto.CredentialUsername = WindowsCredential.UserName;
                        dto.CredentialPassword = WindowsCredential.ExtractPassword();
                        break;
                    case NewAssetRobotValue.CredentialSet:
                        dto.ValueType = AssetDtoValueType.Credential;
                        dto.CredentialUsername = Credential.UserName;
                        dto.CredentialPassword = Credential.ExtractPassword();
                        break;
                }
            }
            else if (dto.ValueScope == AssetDtoValueScope.PerRobot && ParameterSetName == AddAsset.RobotValuesSet)
            {
                dto.RobotValues = dto.RobotValues.MergeAddRemove(
                    AddRobotValues?.Select(rv => rv.ToDto()),                                        // Robot values to add to the list
                    RemoveRobotIdValues?.Select(rid => new AssetRobotValueDto { RobotId = rid }),    // Robot IDs to remove, expressed as AssetRobotValue for sake of MergeAddRemove.. IEnumerable<T> ...
                    rv => rv.RobotId)                                                               // T->K Key selector expression
                    .ToList();
            }
            else
            {
                throw new RuntimeException("Mismatched parameters and asset scope");
            }
            HandleHttpOperationException(() => Api.Assets.PutById(dto.Id.Value, dto));
        }
    }
}
