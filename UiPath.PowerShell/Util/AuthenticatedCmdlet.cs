using Microsoft.Rest;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using UiPath.PowerShell.Cmdlets;
using UiPath.PowerShell.Models;
using UiPath.Web.Client.Generic;
using UiPathWebApi_19_4 = UiPath.Web.Client20194.UiPathWebApi;
using UiPathWebApi_19_10 = UiPath.Web.Client201910.UiPathWebApi;
using UiPathWebApi_20_4 = UiPath.Web.Client20204.UiPathWebApi;
using UiPathWebApi_20_10 = UiPath.Web.Client20204.UiPathWebApi;

namespace UiPath.PowerShell.Util
{
    public abstract class AuthenticatedCmdlet : UiPathCmdlet
    {
        [Parameter()]
        public AuthToken AuthToken { get; set; }

        /// <summary>
        /// The Orchestrator request timeout (in seconds)
        /// </summary>
        [Parameter()]
        public int RequestTimeout { get; set; } = 100;

        private UiPathWebApi_19_4 _api_19_4;
        private UiPathWebApi_19_10 _api_19_10;
        private UiPathWebApi_20_4 _api_20_4;
        private UiPathWebApi_20_10 _api_20_10;

        private TimeSpan Timeout
        {
            get
            {
                if (!MyInvocation.BoundParameters.ContainsKey(nameof(RequestTimeout))
                    && InternalAuthToken.RequestTimeout.HasValue)
                {
                    // If current cmdlet was not passed `-RequestTimeout` but the parama/session auth token did
                    // Use the param/session auth token request timeout
                    return TimeSpan.FromSeconds(InternalAuthToken.RequestTimeout.Value);
                }
                else
                {
                    // Else us the provided RequestTimeout or the default value (100)
                    return TimeSpan.FromSeconds(RequestTimeout);
                }
            }
        }

        internal static AuthToken SessionAuthToken { get; set; }

        protected AuthToken InternalAuthToken
        {
            get
            {
                var authToken = AuthToken ?? SessionAuthToken;
                if (authToken == null)
                {
                    throw new Exception($"An authentication token is required. Specify a value for -AuthToken on the cmdlet or call Set-{Nouns.AuthToken} for the session");
                }
                return authToken;
            }
        }

        internal bool Supports(Version minVersion)
        {
            return Supports(minVersion, InternalAuthToken);
        }

        protected static bool Supports(Version minVersion, AuthToken authToken)
        {
            return authToken.ApiVersion >= minVersion; 
        }

        internal UiPathWebApi_19_4 Api => Api_19_4;

        internal UiPathWebApi_19_4 Api_19_4
        {
            get
            {
                if (_api_19_4 == null)
                {
                    var authToken = InternalAuthToken;
                    _api_19_4 = MakeApi<UiPathWebApi_19_4>(authToken, (creds, uri) => new UiPathWebApi_19_4(creds) { BaseUri = uri }, Timeout);
                }
                return _api_19_4;
            }
        }
        
        internal UiPathWebApi_19_10 Api_19_10
        {
            get
            {
                if (_api_19_10 == null)
                {
                    var authToken = InternalAuthToken;
                    _api_19_10 = MakeApi<UiPathWebApi_19_10>(authToken, (creds, uri) => new UiPathWebApi_19_10(creds) { BaseUri = uri }, Timeout);
                }
                return _api_19_10;
            }
        }

        internal UiPathWebApi_20_4 Api_20_4
        {
            get
            {
                if (_api_20_4 == null)
                {
                    var authToken = InternalAuthToken;
                    _api_20_4 = MakeApi<UiPathWebApi_20_4>(authToken, (creds, uri) => new UiPathWebApi_20_4(creds) { BaseUri = uri }, Timeout);
                }
                return _api_20_4;
            }
        }

        internal UiPathWebApi_20_10 Api_20_10
        {
            get
            {
                if (_api_20_10 == null)
                {
                    var authToken = InternalAuthToken;
                    _api_20_10 = MakeApi<UiPathWebApi_20_10>(authToken, (creds, uri) => new UiPathWebApi_20_10(creds) { BaseUri = uri }, Timeout);
                }
                return _api_20_10;
            }
        }

        public static UiPathWebApi_19_4 MakeApi(AuthToken authToken, TimeSpan timeout)
        {
            return MakeApi<UiPathWebApi_19_4>(authToken, (creds, uri) => new UiPathWebApi_19_4(creds) { BaseUri = uri }, timeout);
        }

        internal static void RefreshAuthToken(AuthToken authToken)
        {
            // https://docs.uipath.com/orchestrator/v2019/reference#section-getting-authorization-code

            // TODO: handle token expiration
            if (!string.IsNullOrWhiteSpace(authToken.Token))
            {
                return;
            }

            var api = new UiPathWebApi_19_4(
                new BasicAuthenticationCredentials())
            {
                BaseUri = new Uri(authToken.AuthorizationUrl)
            };

            JObject req = string.IsNullOrWhiteSpace(authToken.AuthorizationRefreshToken) ?
                new JObject(
                    new JProperty("grant_type", "authorization_code"),
                    new JProperty("redirect_uri", $"{authToken.AuthorizationUrl}/mobile"),
                    new JProperty("client_id", authToken.ApplicationId))
                : new JObject(
                    new JProperty("grant_type", "refresh_token"),
                    new JProperty("refresh_token", authToken.AuthorizationRefreshToken),
                    new JProperty("client_id", authToken.ApplicationId));

            api.MakeHttpRequest(
                HttpMethod.Post,
                "oauth/token",
                req,
                out var response,
                out var headers);

            var jr = JToken.Parse(response);
            var accessToken = jr.Value<string>("access_token");
            var refreshToken = authToken.AuthorizationRefreshToken ?? jr.Value<string>("refresh_token");
            var idToken = jr.Value<string>("id_token");

            if (string.IsNullOrWhiteSpace(accessToken)
                || string.IsNullOrWhiteSpace(refreshToken)
                || string.IsNullOrWhiteSpace(idToken))
            {
                throw new ApplicationException("The authorization response is missing the required tokens");
            }

            api = new UiPathWebApi_19_4(
                new TokenCredentials(idToken))
            {
                BaseUri = new Uri(authToken.AccountUrl)
            };

            string accountLogicalName = authToken.AccountName;
            string tenantLogicalName = authToken.TenantName;

            authToken.Token = accessToken;
            authToken.AuthorizationRefreshToken = refreshToken;
            authToken.AuthorizationTokenId = idToken;
            authToken.URL = $"{authToken.AccountUrl}/{accountLogicalName}/{tenantLogicalName}/";
            authToken.TenantName = tenantLogicalName;
            authToken.AccountName = accountLogicalName;
        }

        internal static UiPathWebApi_19_10 MakeApi_19_10(AuthToken authToken, TimeSpan timeout) => MakeApi<UiPathWebApi_19_10>(
            authToken,
            (creds, uri) => new UiPathWebApi_19_10(creds) { BaseUri = uri },
            timeout);

        internal static T MakeApi<T>(AuthToken authToken, Func<ServiceClientCredentials, Uri, T> ctor, TimeSpan timeout) where T:ServiceClient<T>, IUiPathWebApi
        {
            ServiceClientCredentials creds = null;
            if (!string.IsNullOrWhiteSpace(authToken.AuthorizationRefreshToken))
            {
                RefreshAuthToken(authToken);
                creds = new TokenCredentials(authToken.Token);
            }
            else if (authToken.WindowsCredentials)
            {
                creds = new NetworkAuthenticationCredentials
                {
                    Credentials = CredentialCache.DefaultNetworkCredentials
                };
            }
            else if (authToken.Authenticated == false)
            {
                creds = new BasicAuthenticationCredentials();
            }
            else
            {
                creds = new TokenCredentials(authToken.Token);
            }

            var api = ctor(creds, new Uri(authToken.URL));
            api.SetRetryPolicy(null);
            api.HttpClient.Timeout = timeout;
            api.DeserializationSettings.Converters.Add(new KeyValuePairConverter());
            if (authToken.OrganizationUnitId.HasValue)
            {
                api.HttpClient.DefaultRequestHeaders.Add("X-UIPATH-OrganizationUnitId", authToken.OrganizationUnitId.Value.ToString());
            }
            if (!string.IsNullOrWhiteSpace(authToken.TenantName))
            {
                api.HttpClient.DefaultRequestHeaders.Add("X-UIPATH-TenantName", authToken.TenantName);
            }
            return api;
        }

        internal static void SetAuthToken(AuthToken authToken)
        {
            SessionAuthToken = authToken;
        }
    }
}
