using System.Management.Automation;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Stop, Nouns.Job)]
    public class StopJob: AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Job")]
        public Models.Job Job { get; set; }


        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "JobId")]
        public long? JobId { get; set; }

        [ValidateEnum(typeof(ProcessScheduleDtoStopStrategy))]
        [Parameter(Mandatory = true, Position = 1)]
        public string Strategy { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.Jobs.StopJobById(Job?.Id ?? JobId.Value, new StopJobParameters
            {
                Strategy = Strategy
            }));
        }
    }
}
