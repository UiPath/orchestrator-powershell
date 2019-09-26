using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.User)]
    public class GetUser: FilteredIdCmdlet
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
        [ValidateEnum(typeof(UserDtoType))]
        [Parameter(ParameterSetName = "Filter")]
        public string Type { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string UserName { get; private set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                (filter, top, skip) => Api.Users.GetUsers(filter: filter, top: top, skip: skip, count: false),
                id => Api.Users.GetById(id),
                dto => User.FromDto(dto));
        }
    }
}
