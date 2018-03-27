using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UiPath.PowerShell.Tests.Util
{
    public class TestClassBase
    {
        protected TestRandom TestRandom = new TestRandom();

        public TestContext TestContext { get; set; }

        public ApiHelper Api => ApiHelper.FromTestContext(TestContext);

    }
}
