using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Management.Automation;
using Cmdlet = System.Management.Automation.PowerShell;

namespace UiPath.PowerShell.Tests.Util
{
    public class TestClassBase
    {
        protected TestRandom TestRandom = new TestRandom();

        public TestContext TestContext { get; set; }

        public ApiHelper Api => ApiHelper.FromTestContext(TestContext);


        public Collection<T> Invoke<T>(Cmdlet cmdlet)
        {
            HookCmdlet(cmdlet, TestContext);
            return cmdlet.Invoke<T>();
        }

        public Collection<PSObject> Invoke(Cmdlet cmdlet)
        {
            HookCmdlet(cmdlet, TestContext);
            return cmdlet.Invoke();
        }

        public static void HookCmdlet(Cmdlet cmdlet, TestContext testContext)
        {
            var streams = cmdlet.Streams;
            streams.Debug.DataAdded += (sender, e) =>
            {
                testContext.WriteLine("DEBUG: {0}", streams.Debug[e.Index].Message);
            };
            streams.Error.DataAdded += (sender, e) =>
            {
                testContext.WriteLine("ERROR: {0}:{1} {2}", streams.Error[e.Index].Exception?.GetType().Name, streams.Error[e.Index].Exception?.Message, streams.Error[e.Index].FullyQualifiedErrorId);
            };
            streams.Information.DataAdded += (sender, e) =>
            {
                testContext.WriteLine("INFO: {0}", streams.Information[e.Index].MessageData);
            };
            streams.Warning.DataAdded += (sender, e) =>
            {
                testContext.WriteLine("WARN: {0} {1}", streams.Warning[e.Index].Message, streams.Warning[e.Index].FullyQualifiedWarningId);
            };
            streams.Verbose.DataAdded += (sender, e) =>
            {
                testContext.WriteLine("VERBOSE: {0}", streams.Verbose[e.Index].Message);
            };
        }
    }
}
