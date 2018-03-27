using Microsoft.Rest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Tests.Util
{
    public class ApiHelper: IDisposable
    {
        private UiPathWebApi _api;

        public void Dispose()
        {
            _api?.Dispose(); ;
        }

        internal static ApiHelper FromTestContext(TestContext testContext)
        {
            var testSettings = TestSettings.FromTestContext(testContext);

            var creds = new BasicAuthenticationCredentials();

            using (var client = new UiPathWebApi(creds)
            {
                BaseUri = new Uri(testSettings.URL)
            })
            {
                var loginModel = new LoginModel
                {
                    TenancyName = testSettings.TenantName,
                    UsernameOrEmailAddress = testSettings.UserName,
                    Password = testSettings.Password
                };
                var response = client.Account.Authenticate(loginModel);
                var token = (string)response.Result;

                var tokenCreds = new TokenCredentials(token);

                var api = new UiPathWebApi(tokenCreds)
                {
                    BaseUri = new Uri(testSettings.URL)
                };

                return new ApiHelper
                {
                    _api = api
                };
            }
        }

        internal void DeleteEnvironmentById(long id)
        {
            _api.Environments.DeleteById(id);
        }

        internal void DeleteAssetById(long id)
        {
            _api.Assets.DeleteById(id);
        }
    }
}
