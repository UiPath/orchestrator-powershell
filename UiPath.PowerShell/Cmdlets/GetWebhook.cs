using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20184;

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
                (filter, top, skip) => Api_18_4.Webhooks.GetWebhooks(filter: filter, top: top, skip: skip, count: false),
                id => Api_18_4.Webhooks.GetById(id),
                dto => Webhook.FromDto(dto));
        }
    }
}
