using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.Library)]
    public class RemoveLibrary : AuthenticatedCmdlet
    {
        [ValidateNotNullOrEmpty]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public string Id { get; set; }
        
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Library", ValueFromPipeline = true)]
        public Library Library { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.Libraries.DeleteById(Library?.Id ?? Id));
        }
    }
}
