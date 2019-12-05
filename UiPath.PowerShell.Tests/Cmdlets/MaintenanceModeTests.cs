using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Tests.Util;

namespace UiPath.PowerShell.Tests.Cmdlets
{
    /// <summary>
    /// Maintenance Mode PowerShell tests
    /// </summary>
    [TestClass]
    public class MaintenanceModeTests : TestClassBase
    {
        private const string drainingPhase = "Draining";
        private const string suspendedPhase = "Suspended";
        private const string onlinePhase = "None";

        [TestMethod]
        [DataRow(null, DisplayName = "Host")]
        [DataRow(1, DisplayName = "Tenant")]
        public void MaintenanceMode_FullCycle_Works(int? tenantId)
        {
            System.Management.Automation.PowerShell CmdletFactory(Runspace r, string command, bool skipConfirm = true)
            {
                var cmdlet = PowershellFactory.CreateCmdlet(r);
                cmdlet.AddCommand(command);
                if (skipConfirm)
                {
                    cmdlet.AddParameter("Confirm", false);
                }
                if (tenantId.HasValue)
                {
                    cmdlet.AddParameter("TenantId", tenantId.Value);
                }
                return cmdlet;
            }

            // Host admin op
            using (var hostRunspace = PowershellFactory.CreateHostAuthenticatedSession(TestContext))
            {
                try
                {
                    using (var cmdlet = CmdletFactory(hostRunspace, UiPathStrings.StartMaintenanceMode))
                    {
                        cmdlet.AddParameter("Phase", drainingPhase);

                        // check results in Draining
                        CheckLogs(Invoke(cmdlet), drainingPhase, 2);
                    }

                    using (var cmdlet = CmdletFactory(hostRunspace, UiPathStrings.StartMaintenanceMode))
                    {
                        cmdlet.AddParameter("Phase", suspendedPhase)
                            .AddParameter("Force", true);

                        // check results in Suspended
                        CheckLogs(Invoke(cmdlet), suspendedPhase, 3);

                    }

                    using (var cmdlet = CmdletFactory(hostRunspace, UiPathStrings.StopMaintenanceMode))
                    {
                        // check logs back online
                        CheckLogs(Invoke(cmdlet), onlinePhase, 4);
                    }

                    // check final results, check Get-Maintenance here as well
                    using (var cmdlet = CmdletFactory(hostRunspace, UiPathStrings.GetMaintenanceMode, false))
                    {
                        // check logs are persisted after stopping
                        CheckLogs(Invoke(cmdlet), onlinePhase, 4);
                    }
                    
                }
                finally
                {
                    // exit maintenance mode
                    try
                    {
                        using (var cmdlet = CmdletFactory(hostRunspace, UiPathStrings.StopMaintenanceMode))
                        {
                            Invoke(cmdlet);
                        }
                    }
                    catch { } // don't fail the test
                }
            }
        }

        private static void CheckLogs(ICollection<PSObject> logs, string phase, int length)
        {
            var result = logs.Single().BaseObject as MaintenanceSetting;
            Assert.IsNotNull(result);
            Assert.AreEqual(phase, result.State);
            Assert.AreEqual(length, result.MaintenanceLogs.Count);
        }
    }
}

