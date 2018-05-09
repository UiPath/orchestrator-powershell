using Microsoft.Rest;
using System;
using System.Management.Automation;
using System.Net;
using UiPath.PowerShell.Cmdlets;
using UiPath.PowerShell.Models;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Util
{
    public  abstract class AuthenticatedCmdlet: UiPathCmdlet
    {
        [Parameter()]
        public AuthToken AuthToken { get; set; }

        private UiPathWebApi _api;

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
            return api;
        }

        internal static void SetAuthToken(AuthToken authToken)
        {
            SessionAuthToken = authToken;
        }
    }
}
