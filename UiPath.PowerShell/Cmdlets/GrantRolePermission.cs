using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsSecurity.Grant, Nouns.RolePermission)]
    public class GrantRolePermission : AuthenticatedCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public Role Role { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string[] Permissions { get; set; }

        protected override void ProcessRecord()
        {
            var dto = Role.ToDto(Role);

            dto.Permissions = dto.Permissions
                .Select(p => p.Name)
                .Union(Permissions)
                .Distinct()
                .Select(p => new PermissionDto
                {
                    Name = p,
                    IsGranted = true,
                    RoleId = dto.Id.Value
                }).ToList();
            HandleHttpOperationException(() => Api.Roles.PutById(dto.Id.Value, dto));
        }
    }
}
