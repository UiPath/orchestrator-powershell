using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.Edit, Nouns.QueueDefinition)]
    public class EditQueueDefinition: EditCmdlet
    {
        private const string QueueDefinitionParameterSet = "QueueDefinition";
        private const string IdParameterSet = "Id";

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = QueueDefinitionParameterSet)]
        public QueueDefinition QueueDefinition{ get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = IdParameterSet)]
        public long Id { get; set; }


        [SetParameter]
        [Parameter]
        public string Description { get; private set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                () => (ParameterSetName == QueueDefinitionParameterSet) ? QueueDefinition.ToDto(QueueDefinition) : HandleHttpOperationException(() => Api.QueueDefinitions.GetById(Id)),
                (queueDto) => HandleHttpOperationException(() => Api.QueueDefinitions.PutById(queueDto.Id.Value, queueDto)));
        }
    }
}
