using System.IO;
using System.Management.Automation;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.Package)]
    public class AddPackage:AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string PackageFile { get; set; }

        protected override void ProcessRecord()
        {
            using (var stream = File.OpenRead(PackageFile))
            {
                HandleHttpOperationException(() => Api.Processes.UploadPackage(stream));
            }
        }
    }
}
