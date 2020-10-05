using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20204;
using UiPath.Web.Client20204.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.FolderMachines)]
    public class RemoveFolderMachines : AuthenticatedCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true)]
        public Folder Folder { get; private set; }

        [ValidateNotNullOrEmpty]
        [Parameter(Mandatory = true)]
        public long[] MachineIds { get; set; }

        protected override void ProcessRecord()
        {
            var folder = Folder ?? InternalAuthToken.CurrentFolder;

            var folderMachines = Api_20_4.Folders.GetMachinesForFolderByKey(folder.Id, filter: GetFolderMachines.IsAssignedtoFolderFilter).Value;

            var removeMachines = MachineIds.Intersect(folderMachines.Select(mf => mf.Id.Value));

            if (removeMachines.Any())
            {
                Api_20_4.Folders.RemoveMachinesFromFolderById(folder.Id, new RemoveMachinesFromFolderParameters
                {
                    MachineIds = removeMachines.Select(id => (long?)id).ToList(),
                });
            }
            else
            {
                WriteVerbose("No machine from the list is assigned to the folder");
            }
        }
    }
}
