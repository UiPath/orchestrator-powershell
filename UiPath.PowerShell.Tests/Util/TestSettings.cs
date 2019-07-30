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
            var url = testContext.GetTestParameter("url");
            var tenantName = testContext.GetTestParameter("tenantName");
            var userName = testContext.GetTestParameter("userName");
            var password = testContext.GetTestParameter("password");
            var hostPassword = testContext.GetTestParameter("hostPassword");

            return new TestSettings
            {
                URL = url,
                TenantName = tenantName,
                UserName = userName,
                Password = password,
                HostPassword = hostPassword

            };
        }
    }
}
