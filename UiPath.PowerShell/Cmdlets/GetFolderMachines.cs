using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20204;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.FolderMachines)]
    public class GetFolderMachines :FilteredIdCmdlet
    {
        internal const string IsAssignedtoFolderFilter = "IsAssignedToFolder eq true";

        private const string FolderParameterSet = "Folder";

        [Parameter(Position = 0, ValueFromPipeline = true, ParameterSetName = FolderParameterSet)]
        public Folder Folder { get; private set; }

        protected override void ProcessRecord()
        {
            var folder = Folder ?? InternalAuthToken.CurrentFolder;

            ProcessImpl(
                (filter, top, skip) => Api_20_4.Folders.GetMachinesForFolderByKey(folder.Id, filter: filter, skip: skip, top: top),
                dto => MachineFolder.FromDto(dto).WithFolder(folder));
        }

        protected override string ImplicitFilter => IsAssignedtoFolderFilter;
    }
}
