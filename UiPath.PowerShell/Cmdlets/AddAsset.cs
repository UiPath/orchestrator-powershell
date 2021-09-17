using System;
using System.Collections;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Security;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Adds an Asset into Orchestrator</para>
    /// <para type="description">This cmdlet can add global asset value or per-robot asset values.</para>
    /// <para type="description">The asset type is deduced from the parameter set used.</para>
    /// <para type="description">To create per robot values, use New-UiPathAssetRobotValue</para>
    /// <example>
    /// <code>Add-UiPathAsset AGlobalTextAsset -TextValue SomeText</code>
    /// <para>Creates a global asset of type text with the value SomeText.</para>
    /// </example>
    /// <example>
    /// <code>$creds = Get-Credential
    /// Add-UiPathAsset AGlobalWindowsCredentialAsset -WindowsCredential $creds</code>
    /// <para>Creates a global asset of type text with the value SomeText.</para>
    /// </example>
    /// </summary>
    [Cmdlet(VerbsCommon.Add, Nouns.Asset)]
    public class AddAsset: AuthenticatedCmdlet
    {
        public const string RobotValuesSet = "RobotValues";
        /// <summary>
        /// <para type="description">The asset name.</para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0)]
        public string Name { get; private set; }

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

        [Parameter(Mandatory = true, ParameterSetName = RobotValuesSet)]
        public AssetRobotValue[] RobotValues { get; set; }

        protected override void ProcessRecord()
        {
            var asset = new AssetDto
            {
                Name = Name,
                ValueScope = AssetDtoValueScope.Global
            };
            switch (ParameterSetName)
            {
                case NewAssetRobotValue.TextValueSet:

                    asset.ValueType = AssetDtoValueType.Text;
                    asset.StringValue = TextValue;
                    break;
                case NewAssetRobotValue.IntValueSet:

                    asset.ValueType = AssetDtoValueType.Integer;
                    asset.IntValue = IntValue;
                    break;
                case NewAssetRobotValue.BoolValueSet:
                    asset.ValueType = AssetDtoValueType.Bool;
                    asset.BoolValue = BoolValue;
                    break;
                case NewAssetRobotValue.DBConnectionStringSet:
                    asset.ValueType = AssetDtoValueType.DBConnectionString;
                    asset.StringValue = DBConnectionString;
                    break;
                case NewAssetRobotValue.HttpConnectionStringSet:

                    asset.ValueType = AssetDtoValueType.HttpConnectionString;
                    asset.StringValue = HttpConnectionString;
                    break;
                case NewAssetRobotValue.KeyValueListSet:
                    asset.ValueType = AssetDtoValueType.KeyValueList;
                    asset.KeyValueList = KeyValueList.ToKeyList();
                    break;
                case NewAssetRobotValue.WindowsCredentialSet:
                    asset.ValueType = AssetDtoValueType.WindowsCredential;
                    asset.CredentialUsername = WindowsCredential.UserName;
                    asset.CredentialPassword = WindowsCredential.ExtractPassword();

                    break;
                case NewAssetRobotValue.CredentialSet:
                    asset.ValueType = AssetDtoValueType.Credential;
                    asset.CredentialUsername = Credential.UserName;
                    asset.CredentialPassword = Credential.ExtractPassword();

                    break;
                case RobotValuesSet:
                    asset.ValueScope = AssetDtoValueScope.PerRobot;
                    if (RobotValues.Any())
                    {
                        asset.ValueType = (AssetDtoValueType)Enum.Parse(typeof(AssetDtoValueType), RobotValues.First().ValueType.ToString());
                        asset.RobotValues = RobotValues.Select(rv => rv.ToDto()).ToList();
                    }
                    break;
            }

            var dto = HandleHttpOperationException(() => Api.Assets.Post(asset));
            WriteObject(Asset.FromDto(dto));
        }

        internal static SecureString MakeSecureString(string s)
        {
            // One way is SecureString.AppendChar
            // Or cheat with NetworkCredential...
            return new NetworkCredential("", s).SecurePassword;
        }
    }
}
