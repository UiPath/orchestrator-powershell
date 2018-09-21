using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.User)]
    public class AddUser : AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Username { get; private set; }

        //[Credential]
        [Parameter]
        public string Password { get; private set; }

        [Parameter]
        public string Name { get; private set; }

        [Parameter]
        public string Surname { get; private set; }

        [Parameter]
        public string EmailAddress { get; private set; }

        [Parameter]
        public List<string> RolesList { get; private set; }

        [Parameter]
        public List<long> OrganizationUnitIds { get; set; }

        [ValidateEnum(typeof(UserDtoType))]
        [Parameter]
        public string Type { get; set; }

        protected override void ProcessRecord()
        {
            var user = new UserDto
            {
                UserName = Username,
                Password = Password,
                Name = Name,
                Surname = Surname,
                EmailAddress = EmailAddress,
                RolesList = RolesList,
                OrganizationUnits = OrganizationUnitIds?.Select(
                    id => new OrganizationUnitDto
                    {
                        Id = id ,
                        DisplayName = id.ToString() // The DisplayName cannot be null or empty string, but the actual value is irelevant
                    }).ToList()
            };
            UserDtoType type;
            if (Enum.TryParse<UserDtoType>(Type, out type))
            {
                user.Type = type;
            }
            var dto = HandleHttpOperationException(() => Api.Users.Post(user));
            WriteObject(User.FromDto(dto));
        }
    }
}
