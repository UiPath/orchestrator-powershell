using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using UiPath.Web.Client20204;
using UiPath.Web.Client20194.Models;

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
        [Parameter(ParameterSetName = "Filter")]
        public string Type { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string UserName { get; private set; }

        protected override void ProcessRecord()
        {
            if (Supports(OrchestratorProtocolVersion.V20_4))
            {
                ProcessImpl(
                    (filter, top, skip) => Api_20_4.Users.Get(filter: filter, top: top, skip: skip, count: false),
                    id => Api_20_4.Users.GetById(id),
                    dto => User.FromDto(dto));
            }
            if (Supports(OrchestratorProtocolVersion.V19_10))
            {
                ProcessImpl(
                    (filter, top, skip) => HandleHttpResponseException(() => Api_19_10.Users.GetUsersWithHttpMessagesAsync(filter: filter, top: top, skip: skip, count: false)),
                    id => HandleHttpResponseException(() => Api_19_10.Users.GetByIdWithHttpMessagesAsync(id)),
                    dto => User.FromDto(dto));

            }
            else
            {
                ProcessImpl(
                    (filter, top, skip) => Api.Users.GetUsers(filter: filter, top: top, skip: skip, count: false),
                    id => Api.Users.GetById(id),
                    dto => User.FromDto(dto));
            }
        }
    }
}
