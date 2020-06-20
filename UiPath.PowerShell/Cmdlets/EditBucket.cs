using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20204;
using UiPath.Web.Client20204.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.Edit, Nouns.Bucket)]
    public class EditBucket : AuthenticatedCmdlet
    {
        internal const string IdSet = nameof(IdSet);
        internal const string BucketSet = nameof(BucketSet);

        [Parameter(Mandatory = true, ParameterSetName = IdSet)]
        public long Id { get; set; }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = BucketSet)]
        public Bucket Bucket { get; set; }

        [SetParameter]
        [Parameter]
        public string Description { get; set; }

        [SetParameter(DtoProperty = nameof(BucketDto.Options))]
        [Parameter]
        public SwitchParameter ReadOnly { get; set; }

        [SetParameter(DtoProperty = nameof(BucketDto.Options))]
        [Parameter]
        public SwitchParameter AuditReadAccess { get; set; }

        protected override void ProcessRecord()
        {
            var dto = HandleHttpOperationException(() => Api_20_4.Buckets.GetById(Bucket?.Id ?? Id));

            bool anyChange;
            (dto, anyChange) = this.ApplySetParameters(dto);

            if (anyChange)
            {
                dto = HandleHttpOperationException(() => Api_20_4.Buckets.PutById(dto.Id.Value, dto));
                WriteObject(dto);
            }
        }
    }
}
