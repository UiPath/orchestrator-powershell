using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.QueueDefinition)]
    public class RemoveQueueDefinition: AuthenticatedCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public long? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "QueueDefinition", ValueFromPipeline = true)]
        public QueueDefinition QueueDefinition { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.QueueDefinitions.DeleteById(QueueDefinition?.Id ?? Id.Value));
        }
    }
}
