using System.Management.Automation;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Job)]
    public class GetJob: FilteredIdCmdlet
    {
        protected override void ProcessRecord()
        {
            ProcessImpl(
                filter => Api.Jobs.GetJobs(filter: filter).Value,
                id => Api.Jobs.GetById(id),
                dto => Models.Job.FromDto(dto));
        }
    }
}
