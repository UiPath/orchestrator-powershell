using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20182;
using UiPath.Web.Client20183;
using UiPath.Web.Client20204;
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
        [Parameter(ParameterSetName = "Filter")]
        public string Type { get; set; }

        protected override void ProcessRecord()
        {
            if (Supports(OrchestratorProtocolVersion.V20_4))
            {
                ProcessImpl(
                    (filter, top, skip) => Api_20_4.Machines.Get(filter: filter, top: top, skip: skip, count: false),
                    id => Api_20_4.Machines.GetById(id).To<UiPath.Web.Client20204.Models.ExtendedMachineDto>(),
                    dto => Machine.FromDto(dto));

            }
            else if (Supports(OrchestratorProtocolVersion.V18_3))
            {
                ProcessImpl(
                    (filter, top, skip) => Api_18_3.Machines.GetMachines(filter: filter, top: top, skip: skip, count: false),
                    id => Api_18_3.Machines.GetById(id),
                    dto => Machine.FromDto(dto));
            }
            else
            {
                ProcessImpl(
                    (filter, top, skip) => Api_18_2.Machines.GetMachines(filter: filter, top: top, skip: skip, count: false),
                    id => Api_18_2.Machines.GetById(id),
                    dto => Machine.FromDto(dto));
            }
        }
    }
}
