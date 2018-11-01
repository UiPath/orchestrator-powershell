using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.QueueDefinition)]
    public class AddQueueDefinition: AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter]
        public string Description { get; set; }

        [Parameter]
        public SwitchParameter AcceptAutomaticallyRetry { get; set; }

        [Parameter]
        public SwitchParameter EnforceUniqueReference { get; set; }

        [Parameter]
        public int? MaxNumberOfRetries { get; set; }

        protected override void ProcessRecord()
        {
            var queue = HandleHttpOperationException(() => Api.QueueDefinitions.Post(new QueueDefinitionDto
            {
                Name = Name,
                AcceptAutomaticallyRetry = AcceptAutomaticallyRetry.ToBool(),
                Description = Description,
                EnforceUniqueReference = EnforceUniqueReference.ToBool(),
                MaxNumberOfRetries = MaxNumberOfRetries
            }));
            WriteObject(QueueDefinition.FromDto(queue));
        }
    }
}
