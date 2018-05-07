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
            foreach(var id in Add)
            {
                HandleHttpOperationException(() => Api.Users.ToggleRoleById(id, new ToggleRoleParameters
                {
                    Role = Role.Name,
                    Toggle = true
                }));
            }
            foreach(var id in Remove)
            {
                HandleHttpOperationException(() => Api.Users.ToggleRoleById(id, new ToggleRoleParameters
                {
                    Role = Role.Name,
                    Toggle = false
                }));
            }
        }
    }
}
