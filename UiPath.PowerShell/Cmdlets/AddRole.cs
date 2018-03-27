using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.Role)]
    public class AddRole : AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        //[Parameter]
        //public string DisplayName { get; set; } 

        [Parameter]
        public string[] Permissions { get; set; }

        [Parameter]
        public SwitchParameter IsEditable { get; set; }

        [Parameter]
        public SwitchParameter IsStatic { get; set; }

        protected override void ProcessRecord()
        {
            var req = new RoleDto
            {
                Name = Name,
                //DisplayName = DisplayName,
                IsEditable = IsEditable.IsPresent,
                IsStatic = IsStatic.IsPresent,
                Permissions = Permissions?.Select(p => new PermissionDto
                {
                    Name = p,
                    IsGranted = true
                }).ToList()
            };

            var resp = Api.Roles.Post(req);
            WriteObject(Role.FromDto(resp));
        }
    }
}
