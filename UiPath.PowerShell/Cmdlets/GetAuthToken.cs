using Microsoft.Rest;
using System;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;
using System.Windows.Forms;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.OAuth;
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
        private const string CloudInteractiveSet = "CloudInteractiveSet";
        private const string CloudCodeSet = "CloudCodeSet";
        private const string CloudAPISet = "CloudAPISet";
        private const string HostSet = "HostSet";

        private const string CurrentSessionSet = "CurrentSession";

        [Parameter(Mandatory = false, ParameterSetName = CloudInteractiveSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudCodeSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudAPISet)]
        [ValidateEnum(typeof(CloudDeployments))]
        public string CloudDeployment { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = CloudAPISet)]
        public string UserKey { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = CloudAPISet)]
        public string ClientId { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = CloudInteractiveSet)]
        public SwitchParameter Private { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = CloudInteractiveSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudCodeSet)]
        public string AuthorizationUrl { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = CloudInteractiveSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudCodeSet)]
        public string AccountUrl { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = CloudInteractiveSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudCodeSet)]
        public string ApplicationId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = CloudCodeSet)]
        public string AuthorizationCode { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = CloudCodeSet)]
        public string AuthorizationVerifier { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = HostSet)]
        public new SwitchParameter Host { get; set; }

        [Parameter(Mandatory = true, ParameterSetName =CurrentSessionSet)]
        public SwitchParameter CurrentSession { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = UserPasswordSet)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = WindowsCredentialsSet)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = UnauthenticatedSet)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = HostSet)]
        public string URL { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = UserPasswordSet)]
        [Parameter(Mandatory = true, ParameterSetName = HostSet)]
        public string Username { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = UserPasswordSet)]
        [Parameter(Mandatory = true, ParameterSetName = HostSet)]
        public string Password { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = WindowsCredentialsSet)]
        public SwitchParameter WindowsCredentials { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = UnauthenticatedSet)]
        public SwitchParameter Unauthenticated { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = UserPasswordSet)]
        [Parameter(Mandatory = false, ParameterSetName = UnauthenticatedSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudInteractiveSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudCodeSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudAPISet)]
        public string TenantName { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = CloudInteractiveSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudCodeSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudAPISet)]
        public string AccountName { get; set; }

        /// <summary>
        /// Sets the current Organization Unit for the authentication token.
        /// This parameter is only valid for ORchestrator deployments with Organization Units feature enabled.
        /// </summary>
        [Parameter(Mandatory = false, ParameterSetName = UserPasswordSet)]
        [Parameter(Mandatory = false, ParameterSetName = WindowsCredentialsSet)]
        [Parameter(Mandatory = false, ParameterSetName = UnauthenticatedSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudInteractiveSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudCodeSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudAPISet)]
        public string OrganizationUnit { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = UserPasswordSet)]
        [Parameter(Mandatory = false, ParameterSetName = WindowsCredentialsSet)]
        [Parameter(Mandatory = false, ParameterSetName = UnauthenticatedSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudInteractiveSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudCodeSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudAPISet)]
        public string FolderPath { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = UserPasswordSet)]
        [Parameter(Mandatory = false, ParameterSetName = WindowsCredentialsSet)]
        [Parameter(Mandatory = false, ParameterSetName = UnauthenticatedSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudInteractiveSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudCodeSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudAPISet)]
        [Parameter(Mandatory = false, ParameterSetName = HostSet)]
        public SwitchParameter Session { get; set; }

        /// <summary>
        /// The Orchestrator request timeout (in seconds)
        /// </summary>
        [Parameter()]
        public int RequestTimeout { get; set; } = 100;

        private TimeSpan Timeout => TimeSpan.FromSeconds(RequestTimeout);

        protected override void ProcessRecord()
        {
            if (ParameterSetName == CurrentSessionSet)
            {
                WriteObject(AuthenticatedCmdlet.SessionAuthToken);
            }
            else
            {
                AuthToken authToken = null;

                if (Host.IsPresent)
                {
                    TenantName = "Host";
                }

                if (ParameterSetName == UserPasswordSet || ParameterSetName == HostSet)
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
                else if (ParameterSetName == CloudInteractiveSet || ParameterSetName == CloudCodeSet || ParameterSetName == CloudAPISet)
                {
                    AuthToken token = null;
                    if (Session.IsPresent)
                    {
                        token = AuthenticatedCmdlet.SessionAuthToken;
                    }
                    token = token ?? new AuthToken();
                    token.AuthorizationCode = AuthorizationCode;
                    token.AuthorizationVerifier = AuthorizationVerifier;
                    token.TenantName = TenantName;
                    token.AccountName = AccountName;
                    token.Token = null;
                    token.AuthorizationRefreshToken = null;
                    token.AuthorizationTokenId = null;
                    token.AuthorizationUrl = AuthorizationUrl;
                    token.AccountUrl = AccountUrl;
                    token.ApplicationId = ApplicationId;

                    if (ParameterSetName == CloudAPISet)
                    {
                        token.AuthorizationRefreshToken = UserKey;
                        token.ApplicationId = ClientId;
                    }

                    token.CloudDeployment = string.IsNullOrWhiteSpace(CloudDeployment) ?
                        CloudDeployments.Production
                        : (CloudDeployments)Enum.Parse(typeof(CloudDeployments), CloudDeployment);

                    if (ParameterSetName == CloudInteractiveSet)
                    {
                        token = GetInteractiveToken(token);
                    }

                    authToken = token;
                }

                GetServerVersion(authToken);

                authToken.TenantName = authToken.TenantName ?? TenantName;

                if (!string.IsNullOrWhiteSpace(OrganizationUnit))
                {
                    if (authToken.ApiVersion >= OrchestratorProtocolVersion.V19_10)
                    {
                        WriteWarning("The use of OrganizationUnit is deprecated and will be removed. Use FolderPath instead.");
                    }
                    SetOrganizationUnit(authToken, OrganizationUnit);
                }
                else if (!string.IsNullOrWhiteSpace(FolderPath))
                {
                    if (authToken.ApiVersion < OrchestratorProtocolVersion.V19_10)
                    {
                        WriteError("Use of FolderPath requires Orchestrator version 19.10 or newer.");
                    }
                    SetCurrentFolder(authToken, FolderPath, Timeout);
                }
                else if (authToken.ApiVersion >= OrchestratorProtocolVersion.V19_10 && 
                    0 != string.Compare(TenantName, "Host", true) &&
                    !Host.IsPresent)
                {
                    WriteVerbose("No FolderPath was specified, using 'Default'");
                    SetCurrentFolder(authToken, "Default", Timeout);
                }
                else
                {
                    authToken.CurrentFolder = default;
                    authToken.OrganizationUnit = default;
                    authToken.OrganizationUnitId = default;
                }

                if (Session.IsPresent)
                {
                    AuthenticatedCmdlet.SetAuthToken(authToken);
                }

                if (MyInvocation.BoundParameters.ContainsKey(nameof(RequestTimeout)))
                {
                    authToken.RequestTimeout = RequestTimeout;
                }

                WriteObject(authToken);
            }
        }

        private void SetOrganizationUnit(AuthToken authToken, string organizationUnit)
        {
            using (var api = AuthenticatedCmdlet.MakeApi(authToken, Timeout))
            {
                var unit = HandleHttpOperationException(() => api.OrganizationUnits.GetOrganizationUnits(filter: $"DisplayName eq '{organizationUnit}'").Value.First(ou => ou.DisplayName == organizationUnit));
                authToken.OrganizationUnit = unit.DisplayName;
                authToken.OrganizationUnitId = unit.Id.Value;
                authToken.CurrentFolder = default;
            }
        }


        private void GetServerVersion(AuthToken authToken)
        {
            authToken.ApiVersion = OrchestratorProtocolVersion.V18_1;

            using (var api = AuthenticatedCmdlet.MakeApi(authToken, Timeout))
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

        private AuthToken GetInteractiveToken(AuthToken existingToken)
        {
            // see https://orchestrator.uipath.com/v2019/reference#consuming-cloud-api

            var loginForm = new LoginForm();

            loginForm.AuthToken = existingToken;

            loginForm.webBrowser.Navigated += WebBrowser_Navigated;
            loginForm.webBrowser.Navigating += WebBrowser_Navigating;

            loginForm.LoginUser(Private.IsPresent);

            return loginForm.AuthToken;
        }

        private void WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            WriteVerbose($"Navigating: {e.Url}");
        }

        private void WebBrowser_Navigated(object sender, System.Windows.Forms.WebBrowserNavigatedEventArgs e)
        {
            WriteVerbose($"Navigated: {e.Url}");
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
