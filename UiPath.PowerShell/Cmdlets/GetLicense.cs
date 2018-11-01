using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20183;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.License)]
    public class GetLicense : AuthenticatedCmdlet
    {
        protected override void ProcessRecord()
        {
            if (Supports(OrchestratorProtocolVersion.V18_3, InternalAuthToken))
            {
                var license = HandleHttpOperationException(() => Api_18_3.Settings.GetLicense());
                WriteObject(License.FromDto(license));

            }
            else
            {
                var license = HandleHttpOperationException(() => Api.Settings.GetLicense());
                WriteObject(License.FromDto(license));
            }
        }
    }
}
