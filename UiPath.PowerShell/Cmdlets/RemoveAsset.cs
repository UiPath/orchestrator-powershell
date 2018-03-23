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
    [Cmdlet(VerbsCommon.Remove, Nouns.Asset)]
    public class RemoveAsset: AuthenticatedCmdlet
    {
         [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
         public long? Id { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Asset", ValueFromPipeline = true)]
        public Asset Asset { get; set; }

        protected override void ProcessRecord()
        {
            Api.Assets.DeleteById(Asset?.Id ?? Id.Value);
        }
    }
}
