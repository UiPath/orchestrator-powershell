using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Asset)]
    public class GetAsset: FilteredCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                filter => Api.Assets.GetAssets(filter: filter).Value,
                id => throw new Exception("Assets API does not implement get by ID (todo)"),
                dto => Asset.FromDto(dto));
        }
    }
}
