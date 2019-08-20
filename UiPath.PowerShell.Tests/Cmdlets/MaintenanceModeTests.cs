using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Tests.Util;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910.Models;
using Cmdlet = System.Management.Automation.PowerShell;

namespace UiPath.PowerShell.Tests.Cmdlets
{
    /// <summary>
    /// Maintenance Mode PowerShell tests
    /// </summary>
    /// <remarks> In order to run these, MaintenanceMode should be enabled in Orchestrator (add key="MaintenanceMode.Enabled" value="true")</remarks>
    [TestClass]
    public class MaintenanceModeTests : TestClassBase
    {
        private const string drainingPhase = "Draining";
        private const string suspendedPhase = "Suspended";
        private const string onlinePhase = "None";

        [TestMethod]
        public void MaintenanceMode_FullCycle_Works()
        {
            // Host admin op
            using (var hostRunspace = PowershellFactory.CreateHostAuthenticatedSession(TestContext))
            {
                ICollection<PSObject> results;
                try
                {
                    using (var cmdlet = PowershellFactory.CreateCmdlet(hostRunspace))
                    {
                        cmdlet.AddCommand(UiPathStrings.StartMaintenanceMode)
                            .AddParameter("Phase", drainingPhase)
                            .AddParameter("Confirm", false);
                        results = Invoke(cmdlet);
                    }

                    // check results in Draining
                    CheckLogs(results, drainingPhase, 2);

                    using (var cmdlet = PowershellFactory.CreateCmdlet(hostRunspace))
                    {
                        cmdlet.AddCommand(UiPathStrings.StartMaintenanceMode)
                            .AddParameter("Phase", suspendedPhase)
                            .AddParameter("Force", true)
                            .AddParameter("Confirm", false);
                        results = Invoke(cmdlet);
                    }

                    // check results in Suspended
                    CheckLogs(results, suspendedPhase, 3);

                    using (var cmdlet = PowershellFactory.CreateCmdlet(hostRunspace))
                    {
                        cmdlet.AddCommand(UiPathStrings.StopMaintenanceMode)
                            .AddParameter("Confirm", false);
                        results = Invoke(cmdlet);
                    }

                    // check logs back online
                    CheckLogs(results, onlinePhase, 4);

                    // check final results, check Get-Maintenance here as well
                    using (var cmdlet = PowershellFactory.CreateCmdlet(hostRunspace))
                    {
                        cmdlet.AddCommand(UiPathStrings.GetMaintenanceMode);
                        results = Invoke(cmdlet);
                    }

                    // check logs are persisted after stopping
                    CheckLogs(results, onlinePhase, 4);
                }
                finally
                {
                    // exit maintenance mode
                    try
                    {
                        using (var cmdlet = PowershellFactory.CreateCmdlet(hostRunspace))
                        {
                            cmdlet.AddCommand(UiPathStrings.StopMaintenanceMode)
                                .AddParameter("Confirm", false); ;
                            Invoke(cmdlet);
                        }
                    }
                    catch { } // don't fail the test
                }
            }
        }

        private static void CheckLogs(ICollection<PSObject> logs, string phase, int length)
        {
            var result = logs.Single() as dynamic;
            Assert.IsNotNull(result);
            Assert.AreEqual(phase, result.State);
            Assert.AreEqual(length, result.MaintenanceLogs.Length);
        }
    }
}

