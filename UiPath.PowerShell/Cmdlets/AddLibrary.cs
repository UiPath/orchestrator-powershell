using System.IO;
using System.Management.Automation;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20183;
using UiPath.Web.Client201910;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.Library)]
    public class AddLibrary : AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string LibraryPackage { get; set; }

        protected override void ProcessRecord()
        {
            using (var fileStream = File.OpenRead(LibraryPackage))
            {
                if (Supports(OrchestratorProtocolVersion.V19_10))
                {
                    HandleHttpOperationException(() => Api_19_10.Libraries.UploadPackage(fileStream));
                }
                else
                {
                    HandleHttpOperationException(() => Api_18_3.Libraries.UploadPackage(fileStream));
                }
            }
        }
    }
}
