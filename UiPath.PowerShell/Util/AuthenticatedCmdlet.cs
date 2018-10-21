using Microsoft.Rest;
using System;
using System.Management.Automation;
using System.Net;
using UiPath.PowerShell.Cmdlets;
using UiPath.PowerShell.Models;
using UiPath.Web.Client;
using UiPathWebApi_18_3 = UiPath.Web.Client20183.UiPathWebApi;

namespace UiPath.PowerShell.Util
{
    public  abstract class AuthenticatedCmdlet: UiPathCmdlet
    {
        [Parameter()]
        public AuthToken AuthToken { get; set; }

        private UiPathWebApi _api;
        private UiPathWebApi_18_3 _api_18_3;

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

        protected UiPathWebApi Api
        {
            get
            {
                if (_api == null)
                {
                    var authToken = InternalAuthToken;
                    _api = MakeApi(authToken);
                }
                return _api;
            }
        }

        protected UiPathWebApi_18_3 Api_18_3
        {
            get
            {
                if (_api_18_3 == null)
                {
                    var authToken = InternalAuthToken;
                    _api_18_3 = MakeApi_18_3(authToken);
                }
                return _api_18_3;
            }
        }

        internal static UiPathWebApi MakeApi(AuthToken authToken)
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

            var api = new UiPathWebApi(creds)
            {
                BaseUri = new Uri(authToken.URL)
            };
            api.SetRetryPolicy(null);
            api.SerializationSettings.Converters.Add(new SpecificItemDtoConverter());
            if (authToken.OrganizationUnitId.HasValue)
            {
                api.HttpClient.DefaultRequestHeaders.Add("X-UIPATH-OrganizationUnitId", authToken.OrganizationUnitId.Value.ToString());
            }
            return api;
        }

        internal static UiPathWebApi_18_3 MakeApi_18_3(AuthToken authToken)
        {
            if (authToken.ApiVersion < OrchestratorProtocolVersion.V18_3)
            {
                throw new ApplicationException($"The required Orchestrator API version is: {OrchestratorProtocolVersion.V18_3}. The supported version is: {authToken.ApiVersion}.");
            }
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

            var api = new UiPathWebApi_18_3(creds)
            {
                BaseUri = new Uri(authToken.URL)
            };
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
