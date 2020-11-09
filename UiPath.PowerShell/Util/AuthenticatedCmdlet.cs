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
using UiPathWebApi_18_1 = UiPath.Web.Client20181.UiPathWebApi;
using UiPathWebApi_18_2 = UiPath.Web.Client20182.UiPathWebApi;
using UiPathWebApi_18_3 = UiPath.Web.Client20183.UiPathWebApi;
using UiPathWebApi_18_4 = UiPath.Web.Client20184.UiPathWebApi;
using UiPathWebApi_19_1 = UiPath.Web.Client20191.UiPathWebApi;
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

        private UiPathWebApi_18_1 _api;
        private UiPathWebApi_18_2 _api_18_2;
        private UiPathWebApi_18_3 _api_18_3;
        private UiPathWebApi_18_4 _api_18_4;
        private UiPathWebApi_19_1 _api_19_1;
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

        internal UiPathWebApi_18_1 Api
        {
            get
            {
                if (_api == null)
                {
                    var authToken = InternalAuthToken;
                    _api = MakeApi<UiPathWebApi_18_1>(authToken, (creds,uri) => new UiPathWebApi_18_1(creds) { BaseUri = uri }, Timeout);
                }
                return _api;
            }
        }

        internal UiPathWebApi_18_2 Api_18_2
        {
            get
            {
                if (_api_18_2 == null)
                {
                    var authToken = InternalAuthToken;
                    _api_18_2 = MakeApi<UiPathWebApi_18_2>(authToken, (creds, uri) => new UiPathWebApi_18_2(creds) { BaseUri = uri }, Timeout);
                }
                return _api_18_2;
            }
        }

        internal UiPathWebApi_18_3 Api_18_3
        {
            get
            {
                if (_api_18_3 == null)
                {
                    var authToken = InternalAuthToken;
                    _api_18_3 = MakeApi<UiPathWebApi_18_3>(authToken, (creds, uri) => new UiPathWebApi_18_3(creds) { BaseUri = uri }, Timeout);
                }
                return _api_18_3;
            }
        }

        internal UiPathWebApi_18_4 Api_18_4
        {
            get
            {
                if (_api_18_4 == null)
                {
                    var authToken = InternalAuthToken;
                    _api_18_4 = MakeApi<UiPathWebApi_18_4>(authToken, (creds, uri) => new UiPathWebApi_18_4(creds) { BaseUri = uri }, Timeout);
                }
                return _api_18_4;
            }
        }

        internal UiPathWebApi_19_1 Api_19_1
        {
            get
            {
                if (_api_19_1 == null)
                {
                    var authToken = InternalAuthToken;
                    _api_19_1 = MakeApi<UiPathWebApi_19_1>(authToken, (creds, uri) => new UiPathWebApi_19_1(creds) { BaseUri = uri }, Timeout);
                }
                return _api_19_1;
            }
        }

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

        public static UiPathWebApi_18_1 MakeApi(AuthToken authToken, TimeSpan timeout)
        {
            return MakeApi<UiPathWebApi_18_1>(authToken, (creds, uri) => new UiPathWebApi_18_1(creds) { BaseUri = uri }, timeout);
        }

        internal static void RefreshAuthToken(AuthToken authToken)
        {
            // https://docs.uipath.com/orchestrator/v2019/reference#section-getting-authorization-code

            // TODO: handle token expiration
            if (!string.IsNullOrWhiteSpace(authToken.Token))
            {
                return;
            }

            var api = new UiPathWebApi_18_1(
                new BasicAuthenticationCredentials())
            {
                BaseUri = new Uri(authToken.AuthorizationUrl)
            };

            JObject req = string.IsNullOrWhiteSpace(authToken.AuthorizationRefreshToken) ?
                new JObject(
                    new JProperty("grant_type", "authorization_code"),
                    new JProperty("code", authToken.AuthorizationCode),
                    new JProperty("redirect_uri", $"{authToken.AuthorizationUrl}/mobile"),
                    new JProperty("code_verifier", authToken.AuthorizationVerifier),
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
            var idToken = authToken.AuthorizationTokenId ?? jr.Value<string>("id_token");

            if (string.IsNullOrWhiteSpace(accessToken)
                || string.IsNullOrWhiteSpace(refreshToken)
                || string.IsNullOrWhiteSpace(idToken))
            {
                throw new ApplicationException("The authorization response is missing the required tokens");
            }

            api = new UiPathWebApi_18_1(
                new TokenCredentials(idToken))
            {
                BaseUri = new Uri(authToken.AccountUrl)
            };

            api.MakeHttpRequest(
                HttpMethod.Get,
                "cloudrpa/api/getAccountsForUser",
                null,
                out response,
                out headers);
            var ja = JObject.Parse(response);

            string accountLogicalName;
            var jt = ja.Value<JArray>("accounts");
            if (!string.IsNullOrWhiteSpace(authToken.AccountName))
            {
                var account = jt.FirstOrDefault(acc =>
                    0 == string.Compare(acc.Value<string>("accountLogicalName"), authToken.AccountName, true));
                if (account == null)
                {
                    throw new ApplicationException($"The requested account {authToken.AccountName} was not found");
                }
                accountLogicalName = account.Value<string>("accountLogicalName");
            }
            else
            {
                if (jt.Count == 0)
                {
                    throw new ApplicationException($"There is no active account for this user");
                }

                if (jt.Count > 1)
                {
                    throw new ApplicationException($"There are multiple accounts for this user. Specify the desired account using -AccountName");
                }

                accountLogicalName = jt[0].Value<string>("accountLogicalName");
            }

            api.MakeHttpRequest(
                HttpMethod.Get,
                $"cloudrpa/api/account/{accountLogicalName}/getAllServiceInstances",
                null,
                out response,
                out headers);

            string tenantLogicalName;

            jt = JArray.Parse(response);
            if (!string.IsNullOrEmpty(authToken.TenantName))
            {
                var tenant = jt.FirstOrDefault(t => 0 == string.Compare(t.Value<string>("serviceInstanceLogicalName"), authToken.TenantName, true));
                if (tenant == null)
                {
                    throw new ApplicationException($"The requested tenant {authToken.TenantName} was not found");
                }
                tenantLogicalName = tenant.Value<string>("serviceInstanceLogicalName");
            }
            else
            {
                if (jt.Count == 0)
                {
                    throw new ApplicationException($"There are no tenants for this account");
                }
                else if (jt.Count > 1)
                {
                    throw new ApplicationException($"There are multiple tenants for this account. Specify the desired tenant using -TenantName");
                }

                tenantLogicalName = jt[0].Value<string>("serviceInstanceLogicalName");
            }

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
            if (!string.IsNullOrWhiteSpace(authToken.AuthorizationCode) || !string.IsNullOrWhiteSpace(authToken.AuthorizationRefreshToken))
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
            api.SerializationSettings.Converters.Add(new SpecificItemDtoConverter());
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
