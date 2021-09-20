using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Webhook)]
    public class GetWebhook : FilteredIdCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Url { get; set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                (filter, top, skip) => Api.Webhooks.GetWebhooks(filter: filter, top: top, skip: skip, count: false),
                id => Api.Webhooks.GetById(id),
                dto => Webhook.FromDto(dto));
        }
    }
}
