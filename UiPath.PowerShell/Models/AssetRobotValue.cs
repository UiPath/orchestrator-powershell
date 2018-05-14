using System.Collections;
using System.Management.Automation;
using UiPath.PowerShell.Util;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Models
{
    public class AssetRobotValue
    {
        public long RobotId { get; internal set; }

        public string TextValue { get; internal set; }

        public int? IntValue { get; internal set; }

        public bool? BoolValue { get; internal set; }

        public PSCredential Credential { get; internal set; }

        public Hashtable KeyValueList { get; internal set; }

        public AssetRobotValueDtoValueType ValueType { get; internal  set; }

        public AssetRobotValueDto ToDto()
        {
            return new AssetRobotValueDto
            {
                RobotId = RobotId,
                StringValue = TextValue,
                IntValue = IntValue,
                BoolValue = BoolValue,
                ValueType = ValueType,
                CredentialUsername = Credential?.UserName,
                CredentialPassword = Credential?.ExtractPassword()
            };
        }
    }
}
