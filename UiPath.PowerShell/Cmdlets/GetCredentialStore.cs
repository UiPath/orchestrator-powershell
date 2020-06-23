using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910;
//NB: the use of 20.4 resource type enum on the 19.10 client is intentional
using ResourceType20204 = UiPath.Web.Client20204.Models.DefaultCredentialStoreDtoResourceType;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.CredentialStore)]
    public class GetCredentialStore : FilteredIdCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Name { get; set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Type { get; set; }

        [ValidateEnum(typeof(ResourceType20204))]
        [Parameter(ParameterSetName = "Defaults", Mandatory = true)]
        public string Default { get; set; }

        protected override void ProcessRecord()
        {
            if (ParameterSetName == "Defaults")
            {
                var id = this.GetDefaultCredentialStore(Default);
                if (id.HasValue)
                {
                    var store = HandleHttpResponseException(() => Api_19_10.CredentialStores.GetByIdWithHttpMessagesAsync(id.Value));
                    WriteObject(CredentialStore.FromDto(store));
                }
            }
            else
            {
                ProcessImpl(
                    (filter, top, skip) => HandleHttpResponseException(() => Api_19_10.CredentialStores.GetCredentialStoresWithHttpMessagesAsync(filter: filter, top: top, skip: skip, count: false)),
                    id => HandleHttpResponseException(() => Api_19_10.CredentialStores.GetByIdWithHttpMessagesAsync(id)),
                    dto => CredentialStore.FromDto(dto));
            }
        }
    }
}
