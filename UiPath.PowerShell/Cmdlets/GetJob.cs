using System.Management.Automation;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Job)]
    public class GetJob: FilteredIdCmdlet
    {
        protected override void ProcessRecord()
        {
            ProcessImpl(
                (filter, top, skip) => Api.Jobs.GetJobs(filter: filter, top: top, skip: skip, count: false),
                id => Api.Jobs.GetById(id),
                dto => Models.Job.FromDto(dto));
        }
    }
}
