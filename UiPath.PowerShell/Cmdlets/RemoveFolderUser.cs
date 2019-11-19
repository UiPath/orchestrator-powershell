using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910;
using UiPath.Web.Client201910.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.FolderUser)]
    public class RemoveFolderUser : AuthenticatedCmdlet
    {
        private const string FolderParameterSet = "Folder";

        [Parameter(Mandatory = true, ParameterSetName = "Id")]
        public long? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = FolderParameterSet)]
        public Folder Folder { get; private set; }

        [Parameter]
        public long UserId { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api_19_10.Folders.RemoveUserFromFolderById(Folder?.Id ?? Id.Value, new RemoveUserFromFolderParameters(UserId)));
        }
    }
}
