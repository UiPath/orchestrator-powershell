using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Set, Nouns.Authentication)]
    public class SetAuthentication: UiPathCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "AuthToken", Position = 0, ValueFromPipeline = true)]
        public AuthToken AuthToken { get; set; }

        protected override void ProcessRecord()
        {
            if (AuthToken != null)
            {
                AuthenticatedCmdlet.SetAuthToken(AuthToken);
            }
        }
    }
}
