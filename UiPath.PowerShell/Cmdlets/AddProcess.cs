using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using Release20194 = UiPath.Web.Client20194.Models.ReleaseDto;
using Release20204 = UiPath.Web.Client20204.Models.ReleaseDto;

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
            if (Supports(OrchestratorProtocolVersion.V20_4))
            {
                var release = new Release20204
                {
                    Name = Name,
                    EnvironmentId = Environment?.Id ?? EnvironmentId,
                    Description = Description,
                    ProcessKey = Package?.Id ?? PackageId,
                    ProcessVersion = Package?.Version ?? PackageVersion,
                    AutoUpdate = AutoUpdate.IsPresent,
                };
                var dto = HandleHttpResponseException(() => Api_20_4.Releases.PostWithHttpMessagesAsync(release));
                WriteObject(Process.FromDto(dto));
            }
            else
            {
                var release = new Release20194
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
