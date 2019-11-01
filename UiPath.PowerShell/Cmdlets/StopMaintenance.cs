using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Resources;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910;

namespace UiPath.PowerShell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Stops a Maintenance session</para>
    /// <para type="description">This cmdlet will end the current Maintenance session for UiPath Orchestrator service.</para>
    /// <example>
    /// <code>Stop-UiPathMaintenance</code>
    /// <para>Stops the current Maintenance Mode session and puts the service back in Online mode.</para>
    /// </example>
    /// </summary>
    [Cmdlet(VerbsLifecycle.Stop, Nouns.Maintenance, ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    public class StopMaintenance : AuthenticatedCmdlet
    {
         protected override void ProcessRecord()
        {
            if (!ShouldProcess(Resource.Maintenance_Description, Resource.Stop_Maintenance_Warning, Resource.Stop_Maintenance_Caption))
            {
                return;
            }

            HandleHttpOperationException(() => Api_19_10.Maintenance.End());

            WriteObject(MaintenanceSetting.FromDto(Api_19_10.Maintenance.Get()));
        }
    }
}