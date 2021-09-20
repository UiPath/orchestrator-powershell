using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.Webhook)]
    public class AddWebhook : AuthenticatedCmdlet 
    {
        private const string AllEventsSet = "AllEvents";
        private const string SpecificEventsSet = "SpecificEvents";

        [Parameter(Mandatory =true)]
        public string Url { get; set; }

        [Parameter]
        public bool? Enabled { get; set; }

        [Parameter]
        public string Secret { get; set; }

        [Parameter]
        public SwitchParameter AllowInsecureSsl { get; set; }

        [Parameter(ParameterSetName=AllEventsSet)]
        public SwitchParameter AllEvents { get; set; }

        [Parameter(ParameterSetName =SpecificEventsSet)]
        public new string[] Events { get; set; }

        protected override void ProcessRecord()
        {
            var dto = new WebhookDto
            {
                Url = Url,
                Secret = Secret,
                Enabled = Enabled ?? true,
                AllowInsecureSsl = AllowInsecureSsl.IsPresent ? AllowInsecureSsl.ToBool() : false,
            };

            if (ParameterSetName == AllEventsSet)
            {
                dto.Events = new List<WebhookEventDto>();
                dto.SubscribeToAllEvents = true;
            }
            else
            {
                dto.Events = Events.Select(e => new WebhookEventDto { EventType = e }).ToList();
                dto.SubscribeToAllEvents = false;
            }

            var webhook = Api_19_4.Webhooks.Post(dto);
            WriteObject(Webhook.FromDto(webhook));
        }
    }
}
