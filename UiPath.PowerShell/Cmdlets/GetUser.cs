using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.User)]
    public class GetUser: FilteredCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string EmailAddress { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string FullName { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public bool? IsActive { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public bool? IsEmailConfirmed { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Name { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Surname { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string TenancyName { get; private set; }

        [Filter]
        [ValidateSet("User", "Robot")]
        [Parameter(ParameterSetName = "Filter")]
        public string Type { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string UserName { get; private set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                filter => Api.Users.GetUsers(filter: filter).Value,
                id => Api.Users.GetById(id),
                dto => User.FromDto(dto));
        }
    }
}
