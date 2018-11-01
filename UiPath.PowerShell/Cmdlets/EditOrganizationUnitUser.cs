using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.Edit, Nouns.OrganizationUnitUser)]
    public class EditOrganizationUnitUser: AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public OrganizationUnit OrganizationUnit { get; set; }

        [Parameter]
        public long[] AddUserIds { get; set; }

        [Parameter]
        public long[] RemoveUserIds { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.OrganizationUnits.SetUsersById((int)OrganizationUnit.Id, new SetUsersParameters
            {
                AddedUserIds = AddUserIds?.Select(id => (long?) id).ToList() ?? new List<long?>(),
                RemovedUserIds = RemoveUserIds?.Select(id => (long?)id).ToList() ?? new List<long?>(),
            }));
        }
    }
}
