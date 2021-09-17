using System.Management.Automation;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Unlock, Nouns.User)]
    public class UnlockUser : UserCmdlet
    {
        protected override void ProcessRecord(long userId) =>
            Api.Users.SetActiveById(userId, new SetUserActiveParameters(true));
    }
}
