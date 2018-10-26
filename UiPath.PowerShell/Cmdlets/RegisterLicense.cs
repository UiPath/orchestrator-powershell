using System.IO;
using System.Management.Automation;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;

namespace UiPath.PowerShell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Uploads a license file to Orchestrator</para>
    /// </summary>
    [Cmdlet(VerbsLifecycle.Register, Nouns.License)]
    public class RegisterLicense: AuthenticatedCmdlet
    {
        /// <summary>
        /// The license file location
        /// </summary>
        [Parameter(Mandatory = true, Position = 0)]
        public string LicenseFile { get; set; }

        protected override void ProcessRecord()
        {
            using (var stream = File.OpenRead(LicenseFile))
            {
                HandleHttpOperationException(() => Api.Settings.UploadLicense(stream));
            }
        }
    }
}