using Microsoft.Rest;
using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Obtains an UiPath authentication token</para>
    /// <para type="description">The authentication token is needed for other UiPath Powershell cmdlets.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, Nouns.AuthToken)]
    public class GetAuthToken: UiPathCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string URL { get; set; }

        [Parameter(Mandatory = false)]
        public string TenantName { get; set; }

        [Parameter(Mandatory = true)]
        public string Username { get; set; }

        [Parameter(Mandatory = true)]
        public string Password { get; set; }

        [Parameter]
        public SwitchParameter Session { get; set; }

        protected override void ProcessRecord()
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
                var response = client.Account.Authenticate(loginModel);
                var token = (string) response.Result;

                var authToken = new AuthToken
                {
                    Token = token,
                    URL = URL
                };

                if (Session.IsPresent)
                {
                    AuthenticatedCmdlet.SetAuthToken(authToken);
                }

                WriteObject(authToken);
            }
        }
    }
}
