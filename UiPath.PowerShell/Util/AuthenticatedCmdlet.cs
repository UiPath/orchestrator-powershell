using Microsoft.Rest;
using System;
using System.Management.Automation;
using System.Net;
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

        protected UiPathWebApi Api
        {
            get
            {
                if (_api == null)
                {
                    var authToken = AuthToken ?? SessionAuthToken;
                    if (authToken == null)
                    {
                        throw new Exception("An authentication token is required. Specify a value for -AuthToken on the cmdlet or call Set-Authentication for the session");
                    }

                    ServiceClientCredentials creds = null;
                    if (authToken.WindowsCredentials)
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

                    _api = new UiPathWebApi(creds)
                    {
                        BaseUri = new Uri(authToken.URL)
                    };
                    _api.SetRetryPolicy(null);
                    _api.SerializationSettings.Converters.Add(new SpecificItemDtoConverter());
                }
                return _api;
            }
        }

        internal static void SetAuthToken(AuthToken authToken)
        {
            SessionAuthToken = authToken;
        }
    }
}
