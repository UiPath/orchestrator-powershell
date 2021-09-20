using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.Edit, Nouns.Webhook)]
    public class EditWebhook : EditCmdlet
    {
        private const string AllEventsSet = "AllEvents";
        private const string SpecificEventsSet = "SpecificEvents";
        private const string IdSet = "Id";
        private const string WebhookSet = "Webhook";

        [ValidateNotNullOrEmpty]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = AllEventsSet)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = SpecificEventsSet)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = IdSet)]
        public long? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = AllEventsSet)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = SpecificEventsSet)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = WebhookSet, ValueFromPipeline = true)]
        public Webhook Webhook { get; set; }

        [SetParameter]
        [Parameter]
        public string Url { get; set; }

        [SetParameter]
        [Parameter]
        public bool? Enabled { get; set; }

        [SetParameter]
        [Parameter]
        public string Secret { get; set; }

        [SetParameter]
        [Parameter]
        public SwitchParameter AllowInsecureSsl { get; set; }

        [SetParameter]
        [Parameter(ParameterSetName = AllEventsSet)]
        [Parameter(ParameterSetName = WebhookSet)]
        [Parameter(ParameterSetName = IdSet)]
        public SwitchParameter AllEvents { get; set; }

        [SetParameter]
        [Parameter(ParameterSetName = WebhookSet)]
        [Parameter(ParameterSetName = IdSet)]
        [Parameter(ParameterSetName = SpecificEventsSet)]
        public new string[] Events { get; set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                () => Api_19_4.Webhooks.GetById(Webhook?.Id ?? Id.Value),
                dto => Api_19_4.Webhooks.PatchById(dto.Id.Value, dto));
        }
    }
}
