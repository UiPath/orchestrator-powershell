using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20182;
using UiPath.Web.Client20183;
using MachineDto20182 = UiPath.Web.Client20182.Models.MachineDto;
using MachineDto20183 = UiPath.Web.Client20183.Models.MachineDto;
using MachineDtoType = UiPath.Web.Client20183.Models.MachineDtoType;

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

        protected override void ProcessRecord()
        {
            if (MyInvocation.BoundParameters.ContainsKey(nameof(Type)))
            {
                ProcessImpl(
                    (filter) => Api_18_3.Machines.GetMachines(filter: filter).Value,
                    id => Api_18_3.Machines.GetById(id),
                    dto => Machine.FromDto(dto));
            }
            else
            {
                ProcessImpl(
                    (filter) => Api_18_2.Machines.GetMachines(filter: filter).Value,
                    id => Api_18_2.Machines.GetById(id),
                    dto => Machine.FromDto(dto));
            }
        }
    }
}
