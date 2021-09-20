using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.Tenant)]
    public class AddTenant: AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Name { get; private set; }

        [Parameter(Mandatory = true)]
        public string AdminEmailAddress { get; private set; }

        [Parameter(Mandatory = true)]
        public string AdminName { get; private set; }

        [Parameter(Mandatory = true)]
        public string AdminPassword { get; private set; }

        [Parameter()]
        public string AdminSurname { get; private set; }

        protected override void ProcessRecord()
        {
            var tenant = new TenantDto
            {
                Name = Name,
                AdminEmailAddress = AdminEmailAddress,
                AdminName = AdminName,
                AdminPassword = AdminPassword,
                AdminSurname = AdminSurname
            };
            var dto = HandleHttpOperationException(() => Api.Tenants.Post(tenant));
            WriteObject(Tenant.ForDto(dto));
        }
    }
}
