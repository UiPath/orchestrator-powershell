using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Cmdlets
{

    [Cmdlet(VerbsCommon.Remove, Nouns.ProcessSchedule)]
    public class RemoveProcessSchedule : AuthenticatedCmdlet
    {
        [ValidateNotNullOrEmpty]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public long? Id { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "ProcessSchedule", ValueFromPipeline = true)]
        public ProcessSchedule ProcessSchedule { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.ProcessSchedules.DeleteById(ProcessSchedule?.Id ?? Id.Value));
        }
    }
}
