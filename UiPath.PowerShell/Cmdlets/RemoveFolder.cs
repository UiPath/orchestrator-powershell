using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.Folder)]
    public class RemoveFolder : AuthenticatedCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public long? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Folder", ValueFromPipeline = true)]
        public Folder Folder { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api_19_10.Folders.DeleteByIdWithHttpMessagesAsync(Folder?.Id ?? Id.Value));
        }
    }
}
