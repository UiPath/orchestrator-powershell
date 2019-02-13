using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20181.Models;

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
    /// <example>
    ///     <code>Get-UiPathAuthToken -URL https://uipath.corpnet -Username &lt;myuser&gt; -Password &lt;mypassword&gt; -OrganizationUnit &lt;MyOrganization&gt; -Session</code>
    ///     <para>Connect to a private Orchestrator using user name and password and selects a current Organization Unit, saves the token for current session.</para>
    /// </example>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, Nouns.AuthToken)]
    public class GetAuthToken: UiPathCmdlet
    {
        private const string UserPasswordSet = "UserPassword";
        private const string WindowsCredentialsSet = "WindowsCredentials";
        private const string UnauthenticatedSet = "Unauthenticated";

        private const string CurrentSessionSet = "CurrentSession";

        [Parameter(Mandatory = true, ParameterSetName =CurrentSessionSet)]
        public SwitchParameter CurrentSession { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = UserPasswordSet)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = WindowsCredentialsSet)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = UnauthenticatedSet)]
        public string URL { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = UserPasswordSet)]
        public string Username { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = UserPasswordSet)]
        public string Password { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = WindowsCredentialsSet)]
        public SwitchParameter WindowsCredentials { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = UnauthenticatedSet)]
        public SwitchParameter Unauthenticated { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = UserPasswordSet)]
        [Parameter(Mandatory = false, ParameterSetName = WindowsCredentialsSet)]
        [Parameter(Mandatory = false, ParameterSetName = UnauthenticatedSet)]
        public string TenantName { get; set; }

        /// <summary>
        /// Sets the current Organization Unit for the authentication token.
        /// This parameter is only valid for ORchestrator deployments with Organization Units feature enabled.
        /// </summary>
        [Parameter(Mandatory = false, ParameterSetName = UserPasswordSet)]
        [Parameter(Mandatory = false, ParameterSetName = WindowsCredentialsSet)]
        [Parameter(Mandatory = false, ParameterSetName = UnauthenticatedSet)]
        public string OrganizationUnit { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = UserPasswordSet)]
        [Parameter(Mandatory = false, ParameterSetName = WindowsCredentialsSet)]
        [Parameter(Mandatory = false, ParameterSetName = UnauthenticatedSet)]
        public SwitchParameter Session { get; set; }

        protected override void ProcessRecord()
        {
            if (ParameterSetName == CurrentSessionSet)
            {
                WriteObject(AuthenticatedCmdlet.SessionAuthToken);
            }
            else
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

                if (!String.IsNullOrWhiteSpace(OrganizationUnit))
                {
                    SetOrganizationUnit(authToken, OrganizationUnit);
                }

                authToken.TenantName = TenantName ?? "Default";

                if (Session.IsPresent)
                {
                    AuthenticatedCmdlet.SetAuthToken(authToken);
                }
            }
        }

        private void SetOrganizationUnit(AuthToken authToken, string organizationUnit)
        {
            using (var api = AuthenticatedCmdlet.MakeApi(authToken))
            {
                var unit = HandleHttpOperationException(() => api.OrganizationUnits.GetOrganizationUnits(filter: $"DisplayName eq '{organizationUnit}'").Value.First(ou => ou.DisplayName == organizationUnit));
                authToken.OrganizationUnit = unit.DisplayName;
                authToken.OrganizationUnitId = unit.Id.Value;
            }
        }

        private void GetServerVersion(AuthToken authToken)
        {
            authToken.ApiVersion = OrchestratorProtocolVersion.V18_1;

            using (var api = AuthenticatedCmdlet.MakeApi(authToken))
            {
                try
                { 
                    api.MakeHttpRequest(HttpMethod.Get, "odata/Settings/UiPath.Server.Configuration.OData.GetAuthenticationSettings", null, out var response, out var headers);
                    if (headers.TryGetValues("api-supported-versions", out var values))
                    {
                        if (Version.TryParse(values.First(), out var version))
                        {
                            authToken.ApiVersion = version;
                        }
                    }
                    // v18.1 client type system cannot parse the response
                    //
                    var dict = Microsoft.Rest.Serialization.SafeJsonConvert.DeserializeObject<Web.Client20182.Models.ResponseDictionaryDto>(response);
                    for(int i=0; i< dict.Keys.Count; ++i)
                    {
                        if (0 != String.Compare(dict.Keys[i], "Build.Version", true))
                        {
                            continue;
                        }
                        authToken.BuildVersion = dict.Values[i];
                        break;
                    }
                }
                catch (Exception e)
                {
                    WriteVerbose($"Error retrieving API version: {e.GetType().Name}: {e.Message}");
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
