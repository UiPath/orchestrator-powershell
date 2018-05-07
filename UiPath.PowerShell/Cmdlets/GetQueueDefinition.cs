using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.QueueDefinition)]
    public class GetQueueDefinition: FilteredIdCmdlet
    {

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Name { get; internal set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Description { get; internal set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public bool? AcceptAutomaticallyRetry { get; internal set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public bool? EnforceUniqueReference { get; internal set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public int? MaxNumberOfRetries { get; internal set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                filter => Api.QueueDefinitions.GetQueueDefinitions(filter: filter).Value,
                id => Api.QueueDefinitions.GetQueueDefinitions(filter: $"Id eq {Id}").Value.First(qd => qd.Id == Id),
                dto => QueueDefinition.FromDto(dto));
        }
    }
}
