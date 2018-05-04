using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.Edit, Nouns.RoleUser)]
    public class EditRoleUser : AuthenticatedCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public int? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Role", ValueFromPipeline = true)]
        public Role Role { get; set; }

        [Parameter]
        public long[] Add { get; set; }

        [Parameter]
        public long[] Remove { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.Roles.SetUsersById(Role?.Id ?? Id.Value, new SetUsersParameters
            {
                AddedUserIds = Add?.Select(id => (long?)id).ToList() ?? new List<long?>(),
                RemovedUserIds = Remove?.Select(id => (long?)id).ToList() ?? new List<long?>(),
            }));
        }
    }
}
