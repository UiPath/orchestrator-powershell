using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.Update, Nouns.Process)]
    public class UpdateProcess : AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "IdPackageVersion")]
        [Parameter(Mandatory = true, ParameterSetName = "IdPackage")]
        [Parameter(Mandatory = true, ParameterSetName = "IdLatest")]
        [Parameter(Mandatory = true, ParameterSetName = "IdRollback")]
        public long Id { get; private set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, ParameterSetName = "ProcessPackageVersion")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, ParameterSetName = "ProcessPackage")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, ParameterSetName = "ProcessLatest")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, ParameterSetName = "ProcessRollback")]
        public Process  Process { get; private set; }

        [Parameter(Mandatory = true, ParameterSetName = "IdPackageVersion")]
        [Parameter(Mandatory = true, ParameterSetName = "ProcessPackageVersion")]
        public string PackageVersion { get; private set; }

        [Parameter(Mandatory = true, ParameterSetName = "IdPackage")]
        [Parameter(Mandatory = true, ParameterSetName = "ProcessPackage")]
        public Package Package { get; private set; }

        [Parameter(Mandatory = true, ParameterSetName = "IdLatest")]
        [Parameter(Mandatory = true, ParameterSetName = "ProcessLatest")]
        public SwitchParameter Latest { get; private set; }

        [Parameter(Mandatory = true, ParameterSetName = "IdRollback")]
        [Parameter(Mandatory = true, ParameterSetName = "ProcessRollback")]
        public SwitchParameter Rollback { get; private set; }


        protected override void ProcessRecord()
        {
            if (ParameterSetName == "IdPackageVersion" || ParameterSetName == "ProcessPackageVersion"
                || ParameterSetName == "IdPackage" || ParameterSetName == "ProcessPackage")
            {
                HandleHttpOperationException(() => Api.Releases.UpdateToSpecificPackageVersionById(
                    Process?.Id ?? Id,
                    new Web.Client20181.Models.SpecificPackageParameters
                    {
                        PackageVersion = Package?.Version ?? PackageVersion
                    }));
            }
            else if (ParameterSetName == "IdLatest" || ParameterSetName == "ProcessLatest")
            {
                HandleHttpOperationException(() => Api.Releases.UpdateToLatestPackageVersionById(Process?.Id ?? Id));
            }
            else if (ParameterSetName == "IdRollback" || ParameterSetName == "ProcessRollback")
            {
                HandleHttpOperationException(() => Api.Releases.RollbackToPreviousReleaseVersionById(Process?.Id ?? Id));
            }
        }
    }
}
