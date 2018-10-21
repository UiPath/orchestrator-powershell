using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20183;
using UiPath.Web.Client20183.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.Machine)]
    public class AddMachine : AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [ValidateEnum(typeof(MachineDtoType))]
        [Parameter]
        public string Type { get; set; }

        [Parameter]
        public int? NonProductionSlots { get; private set; }

        [Parameter]
        public int? UnattendedSlots { get; private set; }

        [Parameter]
        public Guid LicenseKey { get; private set; }

        protected override void ProcessRecord()
        {
            var machine = new MachineDto
            {
                Name = Name,
                NonProductionSlots = NonProductionSlots,
                UnattendedSlots = UnattendedSlots,
                Type = (MachineDtoType)Enum.Parse(typeof(MachineDtoType), Type ?? nameof(MachineDtoType.Standard))
            };
            if (MyInvocation.BoundParameters.ContainsKey(nameof(LicenseKey)))
            {
                machine.LicenseKey = LicenseKey.ToString();
            }
            var response = HandleHttpOperationException(() => Api_18_3.Machines.Post(machine));
            WriteObject(Machine.FromDto(response));
        }
    }
}
