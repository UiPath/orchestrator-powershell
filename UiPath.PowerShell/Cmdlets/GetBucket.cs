using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20204;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Bucket)]
    public class GetBucket : FilteredIdCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Name { get; set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string StorageProvider { get; set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                (filter, top, skip) => Api_20_4.Buckets.Get(filter: filter, top: top, skip: skip, count: false),
                id => Api_20_4.Buckets.GetById(id),
                dto => Bucket.FromDto(dto));
        }
    }
}
