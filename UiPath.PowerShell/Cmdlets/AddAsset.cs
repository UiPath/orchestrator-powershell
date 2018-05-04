using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Security;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Adds an Asset into Orchestrator</para>
    /// <para type="description">This cmdlet can add global asset value or per-robot asset values.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Add, Nouns.Asset)]
    public class AddAsset: AuthenticatedCmdlet
    {
        /// <summary>
        /// <para type="description">The asset name.</para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0)]
        public string Name { get; private set; }

        [Parameter(Mandatory = true, ParameterSetName = "TextValue")]
        public string TextValue { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "IntValue")]
        public int? IntValue { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "DBConnectionString")]
        public string DBConnectionString { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "HttpConnectionString")]
        public string HttpConnectionString { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "BoolValue")]
        public bool? BoolValue { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "KeyValueList")]
        public Hashtable KeyValueList { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "WindowsCredential")]
        public PSCredential WindowsCredential { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Credential")]
        public PSCredential Credential { get; set; }

        [ValidateEnum(typeof(AssetDtoValueType))]
        [Parameter(Mandatory = true, ParameterSetName = "RobotValues")]
        public string ValueType { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "RobotValues")]
        public Hashtable RobotValues { get; set; }

        protected override void ProcessRecord()
        {
            var asset = new AssetDto
            {
                Name = Name,
                ValueScope = AssetDtoValueScope.Global
            };
            if (TextValue != null)
            {
                asset.ValueType = AssetDtoValueType.Text;
                asset.StringValue = TextValue;
            }
            else if (IntValue.HasValue)
            {
                asset.ValueType = AssetDtoValueType.Integer;
                asset.IntValue = IntValue;
            }
            else if (BoolValue.HasValue)
            {
                asset.ValueType = AssetDtoValueType.Bool;
                asset.BoolValue = BoolValue;
            }
            else if (DBConnectionString != null)
            {
                asset.ValueType = AssetDtoValueType.DBConnectionString;
                asset.StringValue = DBConnectionString;
            }
            else if (HttpConnectionString != null)
            {
                asset.ValueType = AssetDtoValueType.HttpConnectionString;
                asset.StringValue = HttpConnectionString;
            }
            else if (KeyValueList != null)
            {
                asset.ValueType = AssetDtoValueType.KeyValueList;
                asset.KeyValueList = HashtableToKeyList(KeyValueList);
            }
            else if (WindowsCredential != null)
            {
                asset.ValueType = AssetDtoValueType.WindowsCredential;
                asset.CredentialUsername = WindowsCredential.UserName;
                asset.CredentialPassword = ExtractSecureString(WindowsCredential.Password);

            }
            else if (Credential != null)
            {
                asset.ValueType = AssetDtoValueType.Credential;
                asset.CredentialUsername = Credential.UserName;
                asset.CredentialPassword = ExtractSecureString(Credential.Password);

            }
            else if (RobotValues != null)
            {
                asset.ValueType = (AssetDtoValueType)Enum.Parse(typeof(AssetDtoValueType), ValueType);
                asset.ValueScope = AssetDtoValueScope.PerRobot;
                asset.RobotValues = RobotValues.Cast<DictionaryEntry>().Select(rv =>
                {
                    var robotAssetValue = new AssetRobotValueDto
                    {
                        RobotId = Int64.Parse(rv.Key.ToString()),
                        ValueType = (AssetRobotValueDtoValueType)Enum.Parse(typeof(AssetRobotValueDtoValueType), ValueType)
                    };
                    switch (asset.ValueType)
                    {
                        case AssetDtoValueType.Text:
                        case AssetDtoValueType.DBConnectionString:
                        case AssetDtoValueType.HttpConnectionString:
                            robotAssetValue.StringValue = rv.Value.ToString();
                            break;
                        case AssetDtoValueType.Bool:
                            robotAssetValue.BoolValue = Boolean.Parse(rv.Value.ToString());
                            break;
                        case AssetDtoValueType.Integer:
                            robotAssetValue.IntValue = Int32.Parse(rv.Value.ToString());
                            break;
                        case AssetDtoValueType.KeyValueList:
                            robotAssetValue.KeyValueList = HashtableToKeyList((Hashtable)rv.Value);
                            break;
                        case AssetDtoValueType.WindowsCredential:
                            {
                                PSCredential ps = (PSCredential)rv.Value;
                                robotAssetValue.CredentialUsername = ps.UserName;
                                robotAssetValue.CredentialPassword = ExtractSecureString(ps.Password);
                            }
                            break;
                    }

                    return robotAssetValue;
                }).ToList();
            }

            var dto = HandleHttpOperationException(() => Api.Assets.Post(asset));
            WriteObject(Asset.FromDto(dto));
        }

        internal static string ExtractSecureString(SecureString ss)
        {
            // Extract the string from SecureString
            // One way is System.Runtime.InteropServices.Marshal.SecureStringToBSTR
            // The cheat way is NetworkCredential
            return new NetworkCredential("", ss).Password;
        }

        internal static SecureString MakeSecureString(string s)
        {
            // One way is SecureString.AppendChar
            // Or cheat with NetworkCredential...
            return new NetworkCredential("", s).SecurePassword;
        }

        public static List<CustomKeyValuePair> HashtableToKeyList(Hashtable ht)
        {
            return ht.Cast<DictionaryEntry>().Select(de => new CustomKeyValuePair
            {
                Key = de.Key.ToString(),
                Value = de.Value.ToString()
            }).ToList();
        }
    }
}
