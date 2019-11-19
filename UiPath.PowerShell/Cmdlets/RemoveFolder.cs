using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.Folder)]
    public class RemoveFolder: AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public Folder Folder { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api_19_10.Folders.DeleteById(Folder.Id));
        }
    }
}
