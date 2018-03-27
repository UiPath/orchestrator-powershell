using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Tests.Util;

namespace UiPath.PowerShell.Tests.Cmdlets
{
    [TestClass]
    public class RoleTests : TestClassBase
    {
        [TestMethod]
        public void RoleAddGetRemove()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                Role role = default(Role);
                var name = TestRandom.RandomAlphaNumeric();
                var displaName = TestRandom.RandomString();

                var permissions = new List<string>()
                {
                    "Robots.View",
                    "Robots.Edit",
                    "Robots.Create",
                    "Robots.Delete"
                };

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.AddUiPathRole)
                        .AddParameter(UiPathStrings.Name, name)
                        //.AddParameter(UiPathStrings.DisplayName, displaName)  -- bugbug: the displayname is ignored by Orchestrator
                        .AddParameter(UiPathStrings.IsEditable)
                        .AddParameter(UiPathStrings.Permissions, permissions);
                    var roles = cmdlet.Invoke<Role>();
                    Validators.ValidateRoleResult(roles, null, name, name, true, false, null);
                    role = roles[0];
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathRole)
                        .AddParameter(UiPathStrings.Name, role.Name);
                    var roles = cmdlet.Invoke<Role>();
                    Validators.ValidateRoleResult(roles, role.Id, name, name, true, false, permissions);
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.RemoveUiPathRole)
                        .AddParameter(UiPathStrings.Id, role.Id);
                    cmdlet.Invoke();
                }

                    using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathRole)
                        .AddParameter(UiPathStrings.Name, role.Name);
                    var roles = cmdlet.Invoke<Role>();
                    Validators.ValidatEmptyResponse(roles);
                }
            }
        }
    }
}
