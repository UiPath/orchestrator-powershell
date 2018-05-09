using Microsoft.Rest;
using System;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Obtains an UiPath authentication token</para>
    /// <para type="description">The authentication token is needed for other UiPath Powershell cmdlets.</para>
    /// <example>
    ///     <code>Get-UiPathAuthToken -URL https://platform.uipath.com -Username &lt;myuser&gt; -Password &lt;mypassword&gt;</code>
    ///     <para>Connect to UiPath public platform Orchestrator, using user name and password.</para>
    /// </example>
    /// <example>
    ///     <code>Get-UiPathAuthToken -URL https://platform.uipath.com -Username &lt;myuser&gt; -Password &lt;mypassword&gt; -Session</code>
    ///     <para>Connect to UiPath public platform Orchestrator, using user name and password and save the token for the current session.</para>
    /// </example>
    /// <example>
    ///     <code>Get-UiPathAuthToken -URL https://uipath.corpnet -WindowsCredentials -Session</code>
    ///     <para>Connect to a private Orchestrator with Windows enabled, using current Windows credentials and save the token for current session.</para>
    /// </example>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, Nouns.AuthToken)]
    public class GetAuthToken: UiPathCmdlet
    {
        private const string UserPasswordSet = "UserPassword";
        private const string WindowsCredentialsSet = "WindowsCredentials";
        private const string UnauthenticatedSet = "Unauthenticated";

        [Parameter(Mandatory = true, Position = 0)]
        public string URL { get; set; }

        [Parameter(Mandatory = false)]
        public string TenantName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = UserPasswordSet)]
        public string Username { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = UserPasswordSet)]
        public string Password { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = WindowsCredentialsSet)]
        public SwitchParameter WindowsCredentials { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = UnauthenticatedSet)]
        public SwitchParameter Unauthenticated { get; set; }

        [Parameter]
        public SwitchParameter Session { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                AuthToken authToken = null;
                if (ParameterSetName == UserPasswordSet)
                {
                    authToken = GetUserToken();
                }
                else if (ParameterSetName == WindowsCredentialsSet)
                {
                    authToken = GetWindowsToken();
                }
                else if (ParameterSetName == UnauthenticatedSet)
                {
                    authToken = GetUnauthenticatedToken();
                }

                GetServerVersion(authToken);

                if (Session.IsPresent)
                {
                    AuthenticatedCmdlet.SetAuthToken(authToken);
                }

                WriteObject(authToken);
            }
            catch(Exception e)
            {
                WriteVerbose(e.ToString());
            }
        }

        private void GetServerVersion(AuthToken authToken)
        {
            using (var api = AuthenticatedCmdlet.MakeApi(authToken))
            {
                api.MakeHttpRequest(HttpMethod.Get, "/odata/$metadata", null, out var response, out var headers);
                if (headers.TryGetValues("api-supported-versions", out var values))
                {
                    authToken.ApiVersion = values.First();
                }
            }
        }

        private AuthToken GetUnauthenticatedToken()
        {
            return new AuthToken
            {
                URL = URL,
                WindowsCredentials = false,
                Authenticated = false
            };
        }

        private AuthToken GetWindowsToken()
        {
            return new AuthToken
            {
                URL = URL,
                WindowsCredentials = true,
                Authenticated = true
            };
        }

        private AuthToken GetUserToken()
        { 
            var creds = new BasicAuthenticationCredentials();
            using (var client = new UiPathWebApi(creds)
            {
                BaseUri = new Uri(URL)
            })
            {
                var loginModel = new LoginModel
                {
                    TenancyName = TenantName,
                    UsernameOrEmailAddress = Username,
                    Password = Password
                };
                var response = HandleHttpOperationException(() => client.Account.Authenticate(loginModel));
                var token = (string) response.Result;

                return new AuthToken
                {
                    Token = token,
                    URL = URL,
                    WindowsCredentials = false,
                    Authenticated = true
                };

            }
        }
    }
}
