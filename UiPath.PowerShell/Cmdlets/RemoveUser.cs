using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.User)]
    public class RemoveUser: AuthenticatedCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public long? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "User", ValueFromPipeline = true)]
        public User User { get; set; }

        protected override void ProcessRecord()
        {
            Api.Users.DeleteById(User?.Id ?? Id.Value);
        }
    }
}
