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
            HookCmdlet(cmdlet);
            return cmdlet.Invoke<T>();
        }

        public Collection<PSObject> Invoke(Cmdlet cmdlet)
        {
            HookCmdlet(cmdlet);
            return cmdlet.Invoke();
        }

        public void HookCmdlet(Cmdlet cmdlet)
        {
            var streams = cmdlet.Streams;
            streams.Debug.DataAdded += (sender, e) =>
            {
                TestContext.WriteLine("DEBUG: {0}", streams.Debug[e.Index].Message);
            };
            streams.Error.DataAdded += (sender, e) =>
            {
                TestContext.WriteLine("ERROR: {0}:{1} {2}", streams.Error[e.Index].Exception?.GetType().Name, streams.Error[e.Index].Exception?.Message, streams.Error[e.Index].FullyQualifiedErrorId);
            };
            streams.Information.DataAdded += (sender, e) =>
            {
                TestContext.WriteLine("INFO: {0}", streams.Information[e.Index].MessageData);
            };
            streams.Warning.DataAdded += (sender, e) =>
            {
                TestContext.WriteLine("WARN: {0} {1}", streams.Warning[e.Index].Message, streams.Warning[e.Index].FullyQualifiedWarningId);
            };
            streams.Verbose.DataAdded += (sender, e) =>
            {
                TestContext.WriteLine("VERBOSE: {0}", streams.Verbose[e.Index].Message);
            };
        }
    }
}
