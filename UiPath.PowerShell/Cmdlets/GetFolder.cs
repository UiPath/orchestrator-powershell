using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client201910;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Folder)]
    public class GetFolder : FilteredIdCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string DisplayName { get; set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string FullyQualifiedName { get; set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
               (filter, top, skip) => Api_19_10.Folders.GetFolders(filter: filter, top: top, skip: skip, count: false),
                id => Api_19_10.Folders.GetById(id),
                dto => Folder.FromDto(dto));
        }
    }
}
