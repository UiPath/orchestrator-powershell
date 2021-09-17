﻿using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add,  Nouns.EnvironmentRobot)]
    public class AddEnvironmentRobot: AuthenticatedCmdlet
    {
        [Parameter(Mandatory = true)]
        public Environment Environment { get; set; }

        [Parameter(Mandatory = true)]
        public Robot Robot { get; set; }

        protected override void ProcessRecord()
        {
            HandleHttpOperationException(() => Api.Environments.AddRobotById(Environment.Id, new AddRobotParameters
            {
                RobotId = Robot.Id.ToString()
            }));
        }
    }
}
