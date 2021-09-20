using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.License)]
    public class GetLicense : AuthenticatedCmdlet
    {
        protected override void ProcessRecord()
        {
            var license = HandleHttpOperationException(() => Api.Settings.GetLicense());
            WriteObject(License.FromDto(license));
        }
    }
}
