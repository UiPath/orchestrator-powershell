using System.Collections;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Used to create a robot asset value</para>
    /// <para type="description">This cmdlet produces no effect on the Orchestrator. Use the returned Robot Asset Value object with the Add-UiPathAsset -RobotValues to actually create the robot asset value.</para>
    /// <example>
    /// <code>$robotId2Value = New-UiPathAssetRobotValue -RobotId 2 -TextValue SomeValue
    /// $robotId4Value = New-UiPathAssetRobotValue -RobotId 4 -TextValue AnotherValue
    /// Add-UiPath MyAsset -RobotValues $robotId2Value,$robotId4Value</code>
    /// <para>Creates two robot asset values and creates an asset with these two values.</para>
    /// <para>Note that all robot values for an asset must have the same type.</para>
    /// </example>
    /// </summary>
    [Cmdlet(VerbsCommon.New, Nouns.AssetRobotValue)]
    public class NewAssetRobotValue: UiPathCmdlet
    {
        public const string TextValueSet = "TextValue";
        public const string IntValueSet = "IntValue";
        public const string BoolValueSet = "BoolValue";
        public const string KeyValueListSet = "KeyValueList";
        public const string DBConnectionStringSet = "DBConnectionString";
        public const string HttpConnectionStringSet = "HttpConnectionString";
        public const string WindowsCredentialSet = "WindowsCredential";
        public const string CredentialSet = "Credential";

        [Parameter(Mandatory = true)]
        public long RobotId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = TextValueSet)]
        public string TextValue { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = DBConnectionStringSet)]
        public string DBConnectionString { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = HttpConnectionStringSet)]
        public string HttpConnectionString { get; set; }


        [Parameter(Mandatory = true, ParameterSetName = IntValueSet)]
        public int? IntValue { get; set; }

        
        [Parameter(Mandatory = true, ParameterSetName = BoolValueSet)]
        public bool? BoolValue { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = KeyValueListSet)]
        public Hashtable KeyValueList { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = CredentialSet)]
        public PSCredential Credential { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = WindowsCredentialSet)]
        public PSCredential WindowsCredential { get; set; }

        protected override void ProcessRecord()
        {
            var assetRobotValue = new AssetRobotValue
            {
                RobotId = RobotId
            };
            switch(ParameterSetName)
            {
                case TextValueSet:
                    assetRobotValue.ValueType = AssetRobotValueDtoValueType.Text;
                    assetRobotValue.TextValue = TextValue;
                    break;
                case IntValueSet:
                    assetRobotValue.ValueType = AssetRobotValueDtoValueType.Integer;
                    assetRobotValue.IntValue = IntValue;
                    break;
                case BoolValueSet:
                    assetRobotValue.ValueType = AssetRobotValueDtoValueType.Bool;
                    assetRobotValue.BoolValue = BoolValue;
                    break;
                case DBConnectionStringSet:
                    assetRobotValue.ValueType = AssetRobotValueDtoValueType.DBConnectionString;
                    assetRobotValue.TextValue = DBConnectionString;
                    break;
                case HttpConnectionStringSet:
                    assetRobotValue.ValueType = AssetRobotValueDtoValueType.HttpConnectionString;
                    assetRobotValue.TextValue = HttpConnectionString;
                    break;
                case KeyValueListSet:
                    assetRobotValue.ValueType = AssetRobotValueDtoValueType.KeyValueList;
                    assetRobotValue.KeyValueList = KeyValueList;
                    break;
                case CredentialSet:
                    assetRobotValue.ValueType = AssetRobotValueDtoValueType.Credential;
                    assetRobotValue.Credential = Credential;
                    break;
                case WindowsCredentialSet:
                    assetRobotValue.ValueType = AssetRobotValueDtoValueType.WindowsCredential;
                    assetRobotValue.Credential = WindowsCredential;
                    break;
            }
            WriteObject(assetRobotValue);
        }
    }
}
