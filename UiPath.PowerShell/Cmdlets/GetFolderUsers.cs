using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.FolderUsers)]
    public class GetFolderUsers : FilteredIdCmdlet
    {
        private const string FolderParameterSet = "Folder";

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = FolderParameterSet)]
        public Folder Folder { get; private set; }

        [Parameter]
        public bool IncludeInherited { get; private set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
             (filter, top, skip) => Api_19_10.Folders.GetUsersForFolderByKeyAndIncludeinherited(Folder?.Id ?? Id.Value, IncludeInherited, filter: filter, top: top, skip: skip),
             dto => UserRole.FromDto(dto));
        }
    }
}
