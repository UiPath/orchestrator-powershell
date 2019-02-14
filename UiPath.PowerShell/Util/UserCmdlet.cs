using System.Management.Automation;
using UiPath.PowerShell.Models;

namespace UiPath.PowerShell.Util
{
    public abstract class UserCmdlet : AuthenticatedCmdlet
    {
        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Id")]
        public long? Id { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "User", ValueFromPipeline = true)]
        public User User { get; set; }

        protected abstract void ProcessRecord(long userId);

        protected override void ProcessRecord() =>
            HandleHttpOperationException(() => ProcessRecord(User?.Id ?? Id.GetValueOrDefault()));
    }
}