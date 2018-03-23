using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, Nouns.Package)]
    public class RemovePackage: AuthenticatedCmdlet
    {
        [ValidateNotNullOrEmpty]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Key")]
        public string Id { get; set; }

        [Parameter(ParameterSetName = "Key")]
        public string Version { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Package", ValueFromPipeline = true)]
        public Package Package { get; set; }

        protected override void ProcessRecord()
        {
            string key = Package?.Key ?? Id;
            if (Package == null && Version != null)
            {
                key += ":" + Version;
            }
            Api.Processes.DeleteById(key);
        }
    }
}
