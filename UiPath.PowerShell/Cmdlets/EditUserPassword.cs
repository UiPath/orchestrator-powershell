using Newtonsoft.Json.Linq;
using System;
using System.Management.Automation;
using System.Net.Http;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.Edit, Nouns.UserPassword)]
    public class EditUserPassword : AuthenticatedCmdlet
    {

        [Parameter(Mandatory = true, ParameterSetName = "Id")]
        public long Id { get; set; }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "User")]
        public User User { get; set; }

        [Parameter(Mandatory = true)]
        public string CurrentPassword { get; set; }

        [Parameter(Mandatory = true)]
        public string NewPassword { get; set; }

        protected override void ProcessRecord()
        {
            if (InternalAuthToken.Authenticated)
            {
                ChangeAuthenticatedUser();
            }
            else
            {
                ChangeUnauthenticatedUser();
            }
        }

        private void ChangeAuthenticatedUser()
        {
            HandleHttpOperationException(() => Api.Users.ChangePasswordById(User?.Id ?? Id, new ChangePasswordDto
            {
                CurrentPassword = CurrentPassword,
                NewPassword = NewPassword
            }));
        }

        /// <summary>
        /// This is the method to update the initial password of a new deployment (admin must change password at first login)
        /// </summary>
        private void ChangeUnauthenticatedUser()
        {

            var dto = new ChangePasswordAccountDto
            {
                UserId = User?.Id ?? Id,
                CurrentPassword = CurrentPassword,
                NewPassword = NewPassword
            };

            string responseContent = null;

            HandleHttpOperationException(() => Api.MakeHttpRequest(HttpMethod.Post, "/account/changepassword", dto, out responseContent, out var headers));

            // On success Orchestrator returns 200 OK and no payload
            // On error it returns 200 OK and payload with the error...
            // {"result":null,"targetUrl":null,"success":false,"error":{"code":400,"message":"Incorrect password.","details":null,"validationErrors":null},"unAuthorizedRequest":false,"__abp":true}
            if (!String.IsNullOrWhiteSpace(responseContent))
            {
                var jo = JObject.Parse(responseContent);
                var errorCode = jo["error"]?["code"].Value<int>();
                var errorMessage = jo["error"]?["message"].Value<string>();
                WriteError(errorCode, errorMessage);
            }
        }
    }
}
