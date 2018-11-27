using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UiPath.PowerShell.Tests.Util
{
    internal class TestSettings
    {
        public string URL { get; set; }
        public string TenantName { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public string HostPassword { get; set; }

        public static TestSettings FromTestContext(TestContext testContext)
        {
            var url = testContext.Properties["url"];
            var tenantName = testContext.Properties["tenantName"];
            var userName = testContext.Properties["userName"];
            var password = testContext.Properties["password"];
            var hostPassword = testContext.Properties["hostPassword"];

            return new TestSettings
            {
                URL = url?.ToString(),
                TenantName = tenantName?.ToString(),
                UserName = userName?.ToString(),
                Password = password?.ToString(),
                HostPassword = hostPassword?.ToString()

            };
        }
    }
}
