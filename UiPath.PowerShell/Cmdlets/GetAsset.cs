using System.Collections.Generic;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Asset)]
    public class GetAsset: FilteredBaseCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                (filter, top, skip) => Api.Assets.GetAssets(filter: filter, expand: "RobotValues", top: top, skip: skip, count: false),
                dto => Asset.FromDto(dto));
        }
    }
}
