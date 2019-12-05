using System.Management.Automation;

namespace UiPath.PowerShell.Util
{
    public abstract class MaintenanceBaseCmdlet : AuthenticatedCmdlet
    {
        protected const string TenantParameterSet = "Tenant";

        /// <summary>
        /// <para type="description"> 
        /// Specified the Tenant for which the maintenance operation is performed
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, Position = 0, ParameterSetName = TenantParameterSet)]
        public int? TenantId { get; protected set; }
    }
}
