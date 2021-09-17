using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.User)]
    public class AddUser : AuthenticatedCmdlet
    {
        private const string UserPwdSet = "UserPwdSet";
        private const string WindowsSet = "WindowsSet";

        [Parameter(Mandatory = true, Position = 0)]
        public string Username { get; private set; }

        //[Credential]
        [Parameter(ParameterSetName =UserPwdSet)]
        public string Password { get; private set; }

        [Parameter(ParameterSetName =UserPwdSet)]
        public string Name { get; private set; }

        [Parameter(ParameterSetName = UserPwdSet)]
        public string Surname { get; private set; }

        [Parameter]
        public string EmailAddress { get; private set; }

        [Parameter]
        public List<string> RolesList { get; private set; }

        [Parameter]
        public List<long> OrganizationUnitIds { get; set; }

        [Parameter(ParameterSetName = WindowsSet)]
        public string Domain { get; private set; }

        protected override void ProcessRecord()
        {
            if (Username.Contains('\\'))
            {
                var parts = Username.Split('\\');
                if (parts.Length != 2)
                {
                    throw new Exception($"The NT username {Username} can only contain one '\\' delimitator.");
                }

                if (string.IsNullOrWhiteSpace(Domain))
                {
                    Domain = parts[0];
                }
            }

            var user = new UserDto
            {
                UserName = Username,
                Password = Password,
                Domain = Domain,
                Name = Name,
                Surname = Surname,
                EmailAddress = EmailAddress,
                RolesList = RolesList ?? new List<string>(),
                OrganizationUnits = OrganizationUnitIds?.Select(
                    id => new OrganizationUnitDto
                    {
                        Id = id ,
                        DisplayName = id.ToString() // The DisplayName cannot be null or empty string, but the actual value is irelevant
                    }).ToList()
            };
            var dto = HandleHttpOperationException(() => Api.Users.Post(user));
            WriteObject(User.FromDto(dto));
        }
    }
}
