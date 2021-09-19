using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using UiPath.Web.Client20194.Models;
using UiPath.Web.Client20204;
using MachineDto20194 = UiPath.Web.Client20194.Models.MachineDto;
using MachineDto20204 = UiPath.Web.Client20204.Models.MachineDto;
using MachineDtoType20204 = UiPath.Web.Client20204.Models.MachineDtoType;


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

        [RequiredVersion(MinVersion = OrchestratorProtocolVersion.sV20_4)]
        [Parameter]
        public int? TestAutomationSlots { get; private set; }

        [RequiredVersion(MinVersion = OrchestratorProtocolVersion.sV20_4)]
        [Parameter]
        public int? HeadlessSlots { get; private set; }

        [Parameter]
        public Guid LicenseKey { get; private set; }

        protected override void ProcessRecord()
        {
            if (Supports(OrchestratorProtocolVersion.V20_4))
            {
                AddMachine20204();
            }
            else
            {
                AddMachine20194();
            }
        }

        private void AddMachine20204()
        {
            var machine = new MachineDto20204
            {
                Name = Name,
                NonProductionSlots = NonProductionSlots,
                UnattendedSlots = UnattendedSlots,
                TestAutomationSlots = TestAutomationSlots,
                HeadlessSlots = HeadlessSlots,
                Type = (MachineDtoType20204)Enum.Parse(typeof(MachineDtoType20204), Type ?? nameof(MachineDtoType20204.Standard))
            };
            if (MyInvocation.BoundParameters.ContainsKey(nameof(LicenseKey)))
            {
                machine.LicenseKey = LicenseKey.ToString();
            }
            var response = HandleHttpOperationException(() => Api_20_4.Machines.Post(machine));
            WriteObject(Machine.FromDto(response));
        }

        private void AddMachine20194()
        { 
            var machine = new MachineDto20194
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
            var response = HandleHttpOperationException(() => Api_19_4.Machines.Post(machine));
            WriteObject(Machine.FromDto(response));
        }
    }
}
