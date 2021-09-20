﻿using System.Management.Automation;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Lock, Nouns.User)]
    public class LockUser : UserCmdlet
    {
        protected override void ProcessRecord(long userId) =>
            Api.Users.SetActiveById(userId, new SetUserActiveParameters(false));
    }
}
