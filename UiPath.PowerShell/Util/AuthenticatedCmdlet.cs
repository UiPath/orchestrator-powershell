using Microsoft.Rest;
using System;
using System.Management.Automation;
using System.Net;
using UiPath.PowerShell.Cmdlets;
using UiPath.PowerShell.Models;
using UiPath.Web.Client.Generic;
using UiPathWebApi_18_1 = UiPath.Web.Client20181.UiPathWebApi;
using UiPathWebApi_18_2 = UiPath.Web.Client20182.UiPathWebApi;
using UiPathWebApi_18_3 = UiPath.Web.Client20183.UiPathWebApi;
using UiPathWebApi_18_4 = UiPath.Web.Client20184.UiPathWebApi;

namespace UiPath.PowerShell.Util
{
    public  abstract class AuthenticatedCmdlet: UiPathCmdlet
    {
        [Parameter()]
        public AuthToken AuthToken { get; set; }

        private UiPathWebApi_18_1 _api;
        private UiPathWebApi_18_2 _api_18_2;
        private UiPathWebApi_18_3 _api_18_3;
        private UiPathWebApi_18_4 _api_18_4;

        private static AuthToken SessionAuthToken { get; set; }

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

        protected bool Supports(Version minVersion)
        {
            return Supports(minVersion, InternalAuthToken);
        }

        protected static bool Supports(Version minVersion, AuthToken authToken)
        {
            return authToken.ApiVersion >= minVersion; 
        }

        protected UiPathWebApi_18_1 Api
        {
            get
            {
                if (_api == null)
                {
                    var authToken = InternalAuthToken;
                    _api = MakeApi<UiPathWebApi_18_1>(authToken, (creds,uri) => new UiPathWebApi_18_1(creds) { BaseUri = uri });
                }
                return _api;
            }
        }

        protected UiPathWebApi_18_2 Api_18_2
        {
            get
            {
                if (_api_18_2 == null)
                {
                    var authToken = InternalAuthToken;
                    _api_18_2 = MakeApi<UiPathWebApi_18_2>(authToken, (creds, uri) => new UiPathWebApi_18_2(creds) { BaseUri = uri });
                }
                return _api_18_2;
            }
        }

        protected UiPathWebApi_18_3 Api_18_3
        {
            get
            {
                if (_api_18_3 == null)
                {
                    var authToken = InternalAuthToken;
                    _api_18_3 = MakeApi<UiPathWebApi_18_3>(authToken, (creds, uri) => new UiPathWebApi_18_3(creds) { BaseUri = uri });
                }
                return _api_18_3;
            }
        }

        protected UiPathWebApi_18_4 Api_18_4
        {
            get
            {
                if (_api_18_4 == null)
                {
                    var authToken = InternalAuthToken;
                    _api_18_4 = MakeApi<UiPathWebApi_18_4>(authToken, (creds, uri) => new UiPathWebApi_18_4(creds) { BaseUri = uri });
                }
                return _api_18_4;
            }
        }

        public static UiPathWebApi_18_1 MakeApi(AuthToken authToken)
        {
            return MakeApi<UiPathWebApi_18_1>(authToken, (creds, uri) => new UiPathWebApi_18_1(creds) { BaseUri = uri });
        }

        internal static T MakeApi<T>(AuthToken authToken, Func<ServiceClientCredentials, Uri, T> ctor) where T:ServiceClient<T>, IUiPathWebApi
        {
            ServiceClientCredentials creds = null;
            if (authToken.Authenticated == false)
            {
                creds = new BasicAuthenticationCredentials();
            }
            else if (authToken.WindowsCredentials)
            {
                creds = new NetworkAuthenticationCredentials
                {
                    Credentials = CredentialCache.DefaultNetworkCredentials
                };
            }
            else
            {
                creds = new TokenCredentials(authToken.Token);
            }

            var api = ctor(creds, new Uri(authToken.URL));
            api.SetRetryPolicy(null);
            api.SerializationSettings.Converters.Add(new SpecificItemDtoConverter());
            if (authToken.OrganizationUnitId.HasValue)
            {
                api.HttpClient.DefaultRequestHeaders.Add("X-UIPATH-OrganizationUnitId", authToken.OrganizationUnitId.Value.ToString());
            }
            return api;
        }

        internal static void SetAuthToken(AuthToken authToken)
        {
            SessionAuthToken = authToken;
        }
    }
}
