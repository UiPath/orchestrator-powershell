using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.Webhook)]
    public class RemoveWebhook : AuthenticatedCmdlet
    {
        [ValidateNotNullOrEmpty]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public long? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Webhook", ValueFromPipeline = true)]
        public Webhook Webhook { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.Webhooks.DeleteById(Webhook?.Id ?? Id.Value));
        }
    }
}
