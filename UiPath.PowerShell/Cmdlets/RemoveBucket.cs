using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20204;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.Bucket)]
    public class RemoveBucket : AuthenticatedCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public long? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Bucket", ValueFromPipeline = true)]
        public Bucket Bucket { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api_20_4.Buckets.DeleteById(Bucket?.Id ?? Id.Value));
        }
    }
}
