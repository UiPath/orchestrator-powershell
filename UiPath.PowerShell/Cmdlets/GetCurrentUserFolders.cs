using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.FolderCurrentUser)]
    public class GetCurrentUserFolders : AuthenticatedCmdlet
    {
        protected override void ProcessRecord()
        {
            var folders = HandleHttpOperationException(() => Api_19_10.FoldersNavigation.GetAllFoldersForCurrentUser());
            foreach (var f in folders)
            {
                WriteObject(ExtendedFolder.FromDto(f));
            }
        }
    }
}
