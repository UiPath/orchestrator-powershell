using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20204;
using UiPath.Web.Client20204.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.FolderMachines)]
    public class AddFolderMachines : AuthenticatedCmdlet
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

            var newMachines = MachineIds.Except(folderMachines.Select(mf => mf.Id.Value));

            if (newMachines.Any())
            {
                Api_20_4.Folders.AssignMachines(new AssignMachinesActionParameters
                {
                    Assignments = new Web.Client20204.Models.MachineAssignmentsDto
                    {
                        MachineIds = newMachines.Select(i => (long?)i).ToList(),
                        FolderIds = new List<long?>() { folder.Id }
                    }
                });
            }
            else
            {
                WriteVerbose("All machines are already assigned to the folder");
            }
        }
    }
}
