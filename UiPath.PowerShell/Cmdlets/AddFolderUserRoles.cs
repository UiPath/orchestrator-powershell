using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910;
using UiPath.Web.Client201910.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.FolderUserRoles)]
    public class AddFolderUserRoles : AuthenticatedCmdlet
    {
        private const string FolderParameterSet = "Folder";

        [Parameter(Mandatory = true, ParameterSetName = "Id")]
        public long? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = FolderParameterSet)]
        public Folder Folder { get; private set; }

        [Parameter]
        public long[] UserIds { get; set; }

        [Parameter]
        public long[] RoleIds { get; set; }


        protected override void ProcessRecord()
        {
            var assignments = new UserAssignmentsDto
            {
                UserIds = UserIds.Select(u => (long?)u).ToList(),
                RolesPerFolder = new List<FolderRolesDto>
                {
                    new FolderRolesDto
                    {
                        FolderId = Folder?.Id ?? Id.Value,
                        RoleIds = RoleIds.Select(r => (int?)r).ToList()
                    }
                }
            };

            HandleHttpOperationException(() => Api_19_10.Folders.AssignUsers(new AssignUsersActionParameters(assignments)));
        }
    }
}
