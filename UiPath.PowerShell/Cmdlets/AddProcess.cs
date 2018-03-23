using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.Process)]
    public class AddProcess:AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter]
        public long EnvironmentId { get; set; }

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

        protected override void ProcessRecord()
        {
            var release = new ReleaseDto
            {
                Name = Name,
                EnvironmentId = Environment?.Id ?? EnvironmentId,
                Description = Description,
                ProcessKey = Package?.Id ?? PackageId,
                ProcessVersion = Package?.Version ?? PackageVersion
            };
            var dto = Api.Releases.Post(release);
            WriteObject(Process.FromDto(dto));
        }
    }
}
