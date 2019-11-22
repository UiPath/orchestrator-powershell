using System;
using System.Management.Automation;
using UiPath.PowerShell.Cmdlets;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910;
using UiPath.Web.Client201910.Models;

[Cmdlet(VerbsCommon.Add, Nouns.Folder)]
public class AddFolder : AuthenticatedCmdlet
{
    [Parameter(Mandatory = true)]
    public string DisplayName { get; set; }

    [Parameter]
    public string Description { get; set; }

    [Parameter(Mandatory = true)]
    [ValidateEnum(typeof(FolderDtoProvisionType))] 
    public string ProvisionType { get; set; }

    [Parameter(Mandatory = true)]
    [ValidateEnum(typeof(FolderDtoPermissionModel))]
    public string PermissionModel { get; set; }

    [Parameter]
    public long? ParentId { get; set; }

    protected override void ProcessRecord()
    {
        var postDto = new FolderDto
        {
            DisplayName = DisplayName,
            Description = Description,
            ProvisionType = (FolderDtoProvisionType)Enum.Parse(typeof(FolderDtoProvisionType), ProvisionType),
            PermissionModel = (FolderDtoPermissionModel)Enum.Parse(typeof(FolderDtoPermissionModel), PermissionModel),
            ParentId = ParentId,
        };

        var response = HandleHttpOperationException(() => Api_19_10.Folders.Post(postDto));
        WriteObject(Folder.FromDto(response));
    }
}