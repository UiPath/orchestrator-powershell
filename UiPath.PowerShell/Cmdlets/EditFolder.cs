using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910;
using UiPath.Web.Client201910.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.Edit, Nouns.Folder)]
    public class EditFolder: EditCmdlet
    {
        private const string FolderParameterSet = "Folder";
        private const string IdParameterSet = "Id";

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = FolderParameterSet)]
        public Folder Folder { get; set; }

        [Parameter(Mandatory = true, Position = 0)]
        public string DisplayName { get; set; }

        [Parameter(Mandatory = false, Position = 1)]
        public string Description { get; set; }
        public RobotProvisionType? ProvisionType { get; set; }
        public FolderPermissionModel? PermissionModel { get; set; }
        public long? ParentId { get; set; }

               protected override void ProcessRecord()
        {
            ProcessImpl(
                () => (HandleHttpOperationException(() => Api_19_10.Folders.GetById(Folder?.Id ?? Id))),
                (folderDto) => HandleHttpOperationException(() => Api_19_10.Folders.PutById(folderDto.Id.Value, folderDto)));
        }
    }
}
