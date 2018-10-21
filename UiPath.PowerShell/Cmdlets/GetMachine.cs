using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20183;
using UiPath.Web.Client20183.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Machine)]
    public class GetMachine : FilteredIdCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Name { get; set; }

        [Filter]
        [ValidateEnum(typeof(MachineDtoType))]
        [Parameter(ParameterSetName = "Filter")]
        public string Type { get; set; }

        [Filter]
        [ValidateNotNull]
        [Parameter(ParameterSetName = "Filter")]
        public Guid LicenseKey { get; set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                (filter) => Api_18_3.Machines.GetMachines(filter: filter).Value,
                id => Api_18_3.Machines.GetById(id),
                dto => Machine.FromDto(dto));
        }
    }
}
