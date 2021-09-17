using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Permission)]
    public class GetPermission : FilteredBaseCmdlet
    {
        protected override void ProcessRecord()
        {
            ProcessImpl(
                (filter) => Api.Permissions.GetPermissions(filter: filter).Value.Where(p => Permission.IsVisiblePermission(p)).ToList(),
                (dto) => Permission.FromDto(dto).Name);
        }
    }
}
