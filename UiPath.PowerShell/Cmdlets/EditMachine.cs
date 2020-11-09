using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20182;
using UiPath.Web.Client20183;
using UiPath.Web.Client20204;
using UiPath.Web.Client20182.Models;
using UiPath.Web.Client20183.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.Edit, Nouns.Machine)]
    public class EditMachine : EditCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, ParameterSetName ="Machine", ValueFromPipeline = true, Position = 0)]
        public Machine Machine { get; private set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, ParameterSetName = "Id")]
        public long Id { get; private set; }

        [ValidateNotNull]
        [Parameter]
        public string Name { get; private set; }

        [Parameter]
        public int NonProductionSlots { get; private set; }

        [ValidateEnum(typeof(MachineDtoType))]
        [Parameter]
        public MachineDtoType Type { get; private set; }

        [Parameter]
        [SetParameter]
        public int UnattendedSlots { get; private set; }

        [Parameter]
        [SetParameter]
        public int TestAutomationSlots { get; private set; }

        [Parameter]
        [SetParameter]
        public int HeadlessSlots { get; private set; }

        protected override void ProcessRecord()
        {
            if (Supports(OrchestratorProtocolVersion.V20_4))
            {
                ProcessImpl(
                    () => ParameterSetName == "Machine" ? Machine.ToDto20204(Machine) : HandleHttpOperationException(() => Api_20_4.Machines.GetById(Id)),
                    newDto => HandleHttpOperationException(() => Api_20_4.Machines.PutById(newDto.Id.Value, newDto)));
            }
            else if (Supports(OrchestratorProtocolVersion.V18_3))
            {
                ProcessImpl(
                    () => ParameterSetName == "Machine" ? Machine.ToDto20183(Machine) : HandleHttpOperationException(() => Api_18_3.Machines.GetById(Id)),
                    newDto => HandleHttpOperationException(() => Api_18_3.Machines.PutById(newDto.Id.Value, newDto)));
            }
            else
            {
                ProcessImpl(
                    () => ParameterSetName == "Machine" ? Machine.ToDto20182(Machine) : HandleHttpOperationException(() => Api_18_2.Machines.GetById(Id)),
                    newDto => HandleHttpOperationException(() => Api_18_2.Machines.PutById(newDto.Id.Value, newDto)));
            }
        }
    }
}
