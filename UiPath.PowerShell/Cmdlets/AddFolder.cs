using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910;
using UiPath.Web.Client201910.Models;

    [Cmdlet(VerbsCommon.Add, Nouns.Folder)]
    public class AddFolder: AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string DisplayName { get; set; }

        [Parameter(Mandatory = false, Position = 1)]
        public string Description { get; set; }
        public RobotProvisionType? ProvisionType { get; set; }
        public FolderPermissionModel? PermissionModel { get; set; }
        public long? ParentId { get; set; }

        protected override void ProcessRecord()
        {
            var postDto = new FolderDto
            {
                DisplayName = DisplayName,
                Description = Description,
                ProvisionType = ProvisionType,
                PermissionModel = PermissionModel,
                ParentId = ParentId,
            };

            var response = HandleHttpOperationException(() => Api_19_10.Folders.Post(postDto));
            WriteObject(Folders.FromDto(response));
        }
    }