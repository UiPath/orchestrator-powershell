using System;
using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsSecurity.Revoke, Nouns.RolePermission)]
    public class RevokeRolePermission : AuthenticatedCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public Role Role { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string[] Permissions { get; set; }

        protected override void ProcessRecord()
        {
            var dto = Role.ToDto(Role);
            bool hasChanges = false;

            foreach(var p in dto.Permissions)
            {
                if (Permissions.Any(s => s.Equals(p.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    p.IsGranted = false;
                    hasChanges = true;
                }
            }
            if (hasChanges)
            {
                Api.Roles.PutById(dto.Id.Value, dto);
            }
        }
    }
}
