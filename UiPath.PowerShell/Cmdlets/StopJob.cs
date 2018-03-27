using System.Management.Automation;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Stop, Nouns.Job)]
    public class StopJob: AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Job")]
        public Models.Job Job { get; set; }


        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "JobId")]
        public long? JobId { get; set; }

        [ValidateSet("SoftStop","Kill")] // There is no StopJob Strategy enum
        [Parameter(Mandatory = true, Position = 1)]
        public string Strategy { get; set; }

        protected override void ProcessRecord()
        {
            Api.Jobs.StopJobById(Job?.Id ?? JobId.Value, new StopJobParameters
            {
                Strategy = Strategy
            });
        }
    }
}
