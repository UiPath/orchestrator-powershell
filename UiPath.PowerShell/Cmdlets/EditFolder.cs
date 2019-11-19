using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910;
using UiPath.Web.Client201910.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.Edit, Nouns.Folder)]
    public class EditFolder : EditCmdlet
    {
        private const string FolderParameterSet = "Folder";
        private const string IdParameterSet = "Id";

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = FolderParameterSet)]
        public Folder Folder { get; private set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = IdParameterSet)]
        public long Id { get; private set; }

        [SetParameter]
        [Parameter(Mandatory = true, Position = 1)]
        public string DisplayName { get; private set; }

        [SetParameter]
        [Parameter]
        public string Description { get; private set; }

        [SetParameter]
        [Parameter]
        [ValidateEnum(typeof(FolderDtoProvisionType))]
        public FolderDtoProvisionType? ProvisionType { get; private set; }

        [SetParameter]
        [Parameter]
        [ValidateEnum(typeof(FolderDtoPermissionModel))]
        public FolderDtoPermissionModel? PermissionModel { get; private set; }

        [SetParameter]
        [Parameter]
        public long? ParentId { get; private set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                () => (HandleHttpOperationException(() => Api_19_10.Folders.GetById(Folder?.Id ?? Id))),
                (folderDto) => HandleHttpOperationException(() => Api_19_10.Folders.PutById(folderDto.Id.Value, folderDto)));
        }
    }
}
