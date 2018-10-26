using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.PackageVersion)]
    public class GetPackageVersion : AuthenticatedCmdlet
    {
        [Parameter(ParameterSetName ="Id", Mandatory = true)]
        public string Id { get; private set; }


        [Parameter(ParameterSetName ="Package", ValueFromPipeline =true, Mandatory = true)]
        public Package Package { get; private set; }

        [Parameter]
        public string Version { get; private set; }

        [Parameter]
        public bool IsActive { get; private set; }

        [Parameter]
        public bool IsLatestVersion { get; private set; }

        protected override void ProcessRecord()
        {
            var dtos = HandleHttpOperationException(() => Api.Processes.GetProcessVersionsByProcessid(Package?.Id ?? Id).Value).AsQueryable();

            // No Orch support for server side filtering, filter client side
            if (MyInvocation.BoundParameters.ContainsKey(nameof(Version)))
            {
                dtos = dtos.Where(p => p.Version == Version);
            }
            if (MyInvocation.BoundParameters.ContainsKey(nameof(IsActive)))
            {
                dtos = dtos.Where(p => p.IsActive == IsActive);
            }
            if (MyInvocation.BoundParameters.ContainsKey(nameof(IsLatestVersion)))
            {
                dtos = dtos.Where(p => p.IsLatestVersion == IsLatestVersion);
            }

            foreach (var dto in dtos)
            {
                WriteObject(Package.FromDto(dto));
            }
        }
    }
}
