using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.OrganizationUnitUser)]
    public class GetOrganizationUnitUser: AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public OrganizationUnit OrganizationUnit { get; set; }

        protected override void ProcessRecord()
        {
            var usrIds = HandleHttpOperationException(() => Api.OrganizationUnits.GetUserIdsForUnitByKey(OrganizationUnit.Id).Value);
            foreach(var userId in usrIds)
            {
                var user = HandleHttpOperationException(() => Api.Users.GetById(userId.Value));
                WriteObject(User.FromDto(user));
            }
        }
    }
}
