using System.Management.Automation;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910.Models;
using UiPath.Web.Client201910;
using UiPath.PowerShell.Resources;
using Newtonsoft.Json;

namespace UiPath.PowerShell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Starts a Maintenance session</para>
    /// <para type="description">This cmdlet will start a Maintenance session for UiPath Orchestrator service.</para>
    /// <example>
    /// <code>Start-UiPathMaintenance -Phase Draining</code>
    /// <para>Starts a Maintenance Mode session and puts the service in Draining mode.</para>
    /// </example>
    /// <example>
    /// <code>Start-UiPathMaintenance -Phase Suspended -KillJobs</code>
    /// <para>Sets a current Maintenance Mode session to Suspended phase, forcing remaining jobs termination</para>
    /// </example>
    /// </summary>
    [Cmdlet("Start", Nouns.Maintenance, ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    public class StartMaintenance : AuthenticatedCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// Indicates the Maintenance Mode phase.
        ///     Draining = Most user and robots API calls will continue to work. A set of API calls whichi would generate additional Robos workloads will generate a '405 - Method not allowed' response.
        ///     Suspended = All user and robots API calls will generate a '503 - Service Unavailable' response.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0)]
        [ValidateSet("Draining", "Suspended")]
        public string Phase { get; private set; }

        /// <summary>
        /// <para type="description"> 
        /// Forces the remaining active robot Jobs to be terminated instead of gracefully stopped. This parameter is only valid when entering the Suspended phase of a Maintenance Session
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter KillJobs { get; private set; }

        /// <summary>
        /// <para type="description"> 
        /// Bypasses all API validations and forces the UiPath Orchestrator service to enter the specifed Maintenance phase.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter Force { get; private set; }

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(Resource.Maintenance_Description, Resource.Start_Maintenance_Warning, string.Format(Resource.Start_Maintenance_Caption, Phase.ToString())))
            {
                return;
            }

            var shouldKillJobs = KillJobs.IsPresent;
            var force = Force.IsPresent;

            HandleHttpOperationException(() => Api_19_10.Maintenance.Start(Phase.ToString(), force, shouldKillJobs));

            WriteObject(Api_19_10.Maintenance.Get().ToDynamic());
        }
    }
}