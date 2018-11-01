using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20181.Models;
using System.Collections;
using System.Collections.Generic;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.Edit, Nouns.ProcessSchedule)]
    public class EditProcessSchedule : EditCmdlet
    {                    
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public List<long?> Ids { get; set; }        

        [SetParameter]
        [Parameter]
        public bool? Enabled { get; private set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.ProcessSchedules.SetEnabled(new SetEnabledParameters(Ids, Enabled.Value)));            
        }
    }

}