using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;

namespace UiPath.PowerShell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Shows the valid time zone names recognized by the Orchestrator.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, Nouns.TimeZones)]
    public class GetTimeZones : AuthenticatedCmdlet
    {
        protected override void ProcessRecord()
        {
            var timeZones = HandleHttpOperationException(() => Api.Settings.GetTimezones());
            foreach(var tz in timeZones.Items)
            {
                WriteObject(Timezone.FromDto(tz));
            }
        }
    }
}
