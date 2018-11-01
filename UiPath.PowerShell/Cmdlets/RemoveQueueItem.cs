using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.QueueItem)]
    public class RemoveQueueItem: AuthenticatedCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public long? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "QueueItem", ValueFromPipeline = true)]
        public QueueItem QueueItem { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.QueueItems.DeleteById(QueueItem?.Id ?? Id.Value));
        }
    }
}
