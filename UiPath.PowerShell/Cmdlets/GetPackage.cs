using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Package)]
    public class GetPackage: FilteredIdCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public bool? IsActive { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public bool? IsLatestVersion { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Key { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string OldVersion { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Title { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Version { get; private set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                filter => Api.Processes.GetProcesses(filter: filter).Value,
                id => throw new Exception("Packages do not implement GetById"),
                dto => Package.FromDto(dto));
        }
    }
}
