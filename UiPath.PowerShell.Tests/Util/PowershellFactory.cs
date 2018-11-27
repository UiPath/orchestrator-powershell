using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;
using System.Reflection;
using UiPath.PowerShell.Util;
using Cmdlet = System.Management.Automation.PowerShell;

namespace UiPath.PowerShell.Tests.Util
{
    internal static class PowershellFactory
    {
        public static InitialSessionState CreateSessionState()
        {
            var iss = InitialSessionState.CreateDefault();
            var uiPathModule = Assembly.GetAssembly(typeof(UiPathCmdlet));
            iss.ImportPSModule(new[] { uiPathModule.Location });
            return iss;
        }

        public static Runspace CreateRunspace(InitialSessionState iss)
        {
            return RunspaceFactory.CreateRunspace(iss);
        }

        public static Cmdlet  CreateCmdlet(Runspace rss)
        {
            var cmdlet = Cmdlet.Create();
            cmdlet.Runspace = rss;
            return cmdlet;
        }

        public static void AuthenticateSession(Runspace rss, string url, string tenantName, string userName, string password, TestContext testContext)
        {
            using (var cmdlet = CreateCmdlet(rss))
            {
                TestClassBase.HookCmdlet(cmdlet, testContext);
                cmdlet.AddCommand(UiPathStrings.GetUiPathAuthToken)
                    .AddParameter(UiPathStrings.URL, url)
                    .AddParameter(UiPathStrings.Username, userName)
                    .AddParameter(UiPathStrings.Password, password)
                    .AddParameter(UiPathStrings.Session);
                if (!string.IsNullOrWhiteSpace(tenantName))
                {
                    cmdlet.AddParameter(UiPathStrings.TenantName, tenantName);
                }
                cmdlet.Invoke();
            }
        }

        public static Runspace CreateAuthenticatedSession(TestContext testContext)
        {
            var testSettings = TestSettings.FromTestContext(testContext);
            var iss = CreateSessionState();
            var runspace = CreateRunspace(iss);

            try
            {
                runspace.Open();
                AuthenticateSession(runspace, testSettings.URL, testSettings.TenantName, testSettings.UserName, testSettings.Password, testContext);
            }
            catch
            {
                runspace.Dispose();
                throw;
            }
            return runspace;
        }

        public static Runspace CreateHostAuthenticatedSession(TestContext testContext)
        {
            var testSettings = TestSettings.FromTestContext(testContext);
            var iss = CreateSessionState();
            var runspace = CreateRunspace(iss);

            try
            {
                runspace.Open();
                AuthenticateSession(runspace, testSettings.URL, "Host", "admin", testSettings.HostPassword, testContext);
            }
            catch
            {
                runspace.Dispose();
                throw;
            }
            return runspace;
        }

    }
}
