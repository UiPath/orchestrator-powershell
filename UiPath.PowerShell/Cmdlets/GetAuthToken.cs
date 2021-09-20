﻿using Microsoft.Rest;
using System;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using UiPath.Web.Client20194.Models;


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
        private const string CloudAPISet = "CloudAPISet";
        private const string HostSet = "HostSet";

        private const string CurrentSessionSet = "CurrentSession";

        [Parameter(Mandatory = false, ParameterSetName = CloudAPISet)]
        [ValidateEnum(typeof(CloudDeployments))]
        public string CloudDeployment { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = CloudAPISet)]
        public string UserKey { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = CloudAPISet)]
        public string ClientId { get; set; }

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
        [Parameter(Mandatory = true, ParameterSetName = CloudAPISet)]
        public string TenantName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = CloudAPISet)]
        public string AccountName { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = UserPasswordSet)]
        [Parameter(Mandatory = false, ParameterSetName = WindowsCredentialsSet)]
        [Parameter(Mandatory = false, ParameterSetName = UnauthenticatedSet)]
        [Parameter(Mandatory = false, ParameterSetName = CloudAPISet)]
        public string FolderPath { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = UserPasswordSet)]
        [Parameter(Mandatory = false, ParameterSetName = WindowsCredentialsSet)]
        [Parameter(Mandatory = false, ParameterSetName = UnauthenticatedSet)]
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

                if (!string.IsNullOrWhiteSpace(URL))
                {
                    var uri = new Uri(URL);
                    if (0 == string.Compare(uri.Host, "alpha.uipath.com", true) ||
                        0 == string.Compare(uri.Host, "staging.uipath.com", true) ||
                        0 == string.Compare(uri.Host, "cloud.uipath.com", true) ||
                        0 == string.Compare(uri.Host, "platform.uipath.com", true))
                    {
                        WriteError("Use -CloudDeployment parameter to connect to UiPath Automation Cloud");
                        return;
                    }
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
                else if (ParameterSetName == CloudAPISet)
                {
                    AuthToken token = null;
                    if (Session.IsPresent)
                    {
                        token = AuthenticatedCmdlet.SessionAuthToken;
                    }
                    token = token ?? new AuthToken();
                    token.TenantName = TenantName;
                    token.AccountName = AccountName;
                    token.Token = null;
                    token.AuthorizationRefreshToken = null;

                    if (ParameterSetName == CloudAPISet)
                    {
                        token.AuthorizationRefreshToken = UserKey;
                        token.ApplicationId = ClientId;
                    }

                    token.CloudDeployment = string.IsNullOrWhiteSpace(CloudDeployment) ?
                        CloudDeployments.Production
                        : (CloudDeployments)Enum.Parse(typeof(CloudDeployments), CloudDeployment);

                    authToken = token;
                }

                GetServerVersion(authToken);

                authToken.TenantName = authToken.TenantName ?? TenantName;

                if (!string.IsNullOrWhiteSpace(FolderPath))
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
                    FindAndSetDefaultFolder(authToken, Timeout);
                    WriteVerbose($"No FolderPath was specified, using {authToken.CurrentFolder?.DisplayName}");
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
                    var dict = Microsoft.Rest.Serialization.SafeJsonConvert.DeserializeObject<Web.Client20194.Models.ResponseDictionaryDto>(response);
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
