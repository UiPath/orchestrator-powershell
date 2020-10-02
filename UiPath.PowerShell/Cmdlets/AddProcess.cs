using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using Release20181 = UiPath.Web.Client20181.Models.ReleaseDto;
using Release201910 = UiPath.Web.Client201910.Models.ReleaseDto;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.Process)]
    public class AddProcess:AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter]
        public long? EnvironmentId { get; set; }

        [Parameter]
        public Environment Environment { get; set; }

        [Parameter]
        public string PackageId { get; set; }

        [Parameter]
        public string PackageVersion { get; set; }

        [Parameter]
        public Package Package { get; set; }

        [Parameter]
        public string Description { get; set; }

        [Parameter]
        public SwitchParameter AutoUpdate { get; set; }

        protected override void ProcessRecord()
        {
            if (Supports(OrchestratorProtocolVersion.V19_10))
            {
                var release = new Release201910
                {
                    Name = Name,
                    EnvironmentId = Environment?.Id ?? EnvironmentId,
                    Description = Description,
                    ProcessKey = Package?.Id ?? PackageId,
                    ProcessVersion = Package?.Version ?? PackageVersion,
                    AutoUpdate = AutoUpdate.IsPresent,
                };
                var dto = HandleHttpResponseException(() => Api_19_10.Releases.PostWithHttpMessagesAsync(release));
                WriteObject(Process.FromDto(dto));
            }
            else
            {
                var release = new Release20181
                {
                    Name = Name,
                    EnvironmentId = Environment?.Id ?? EnvironmentId.Value,
                    Description = Description,
                    ProcessKey = Package?.Id ?? PackageId,
                    ProcessVersion = Package?.Version ?? PackageVersion
                };
                var dto = HandleHttpOperationException(() => Api.Releases.Post(release));
                WriteObject(Process.FromDto(dto));
            }
        }
    }
}
