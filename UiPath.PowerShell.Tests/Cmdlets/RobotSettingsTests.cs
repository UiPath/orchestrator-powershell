using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UiPath.PowerShell.Cmdlets;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Tests.Util;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Tests.Cmdlets
{
    [TestClass]
    public class RobotSettingsTests : TestClassBase
    {
        [TestMethod]
        public void RobotSettingsSetThenClear()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                var name = TestRandom.RandomString();
                var description = TestRandom.RandomString();
                var machine = TestRandom.RandomString();
                var username = TestRandom.RandomString();
                long? robotId = null;
                License license = null;

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathLicense);
                    license = Invoke<License>(cmdlet)?.FirstOrDefault();

                    Assert.IsNotNull(license);
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.AddUiPathRobot)
                        .AddParameter(UiPathStrings.Name, name)
                        .AddParameter(UiPathStrings.MachineName, machine)
                        .AddParameter(UiPathStrings.LicenseKey, license.Code)
                        .AddParameter(UiPathStrings.Description, description)
                        .AddParameter(UiPathStrings.Username, username)
                        .AddParameter(UiPathStrings.Type, RobotDtoType.NonProduction);
                    var robots = Invoke<Robot>(cmdlet);
                    
                    robotId = robots[0].Id;
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.EditUiPathRobotSettings)
                        .AddParameter(nameof(EditRobotSettings.Id), robotId)
                        .AddParameter(nameof(EditRobotSettings.StudioNotifyServer), false)
                        .AddParameter(nameof(EditRobotSettings.ResolutionHeight), 1920)
                        .AddParameter(nameof(EditRobotSettings.TracingLevel), "Verbose")
                        .AddParameter(nameof(EditRobotSettings.LoginToConsole), true);
                    Invoke<Robot>(cmdlet);
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathRobotSettings)
                        .AddParameter(UiPathStrings.Id, robotId);
                    var robotSettings = Invoke<RobotExecutionSettings>(cmdlet)?.FirstOrDefault();

                    Assert.IsNotNull(robotSettings);
                    Assert.AreEqual(robotId, robotSettings.Id);
                    Assert.AreEqual(false, robotSettings.StudioNotifyServer);
                    Assert.AreEqual(1920, robotSettings.ResolutionHeight);
                    Assert.AreEqual("Verbose", robotSettings.TracingLevel);
                    Assert.AreEqual(true, robotSettings.LoginToConsole);
                    Assert.IsNull(robotSettings.FontSmoothing);
                    Assert.IsNull(robotSettings.ResolutionWidth);
                    Assert.IsNull(robotSettings.ResolutionDepth);
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.ClearUiPathRobotSettings)
                        .AddParameter(UiPathStrings.Id, robotId)
                        .AddParameter(nameof(ClearRobotSettings.StudioNotifyServer))
                        .AddParameter(nameof(ClearRobotSettings.TracingLevel))
                        .AddParameter(nameof(ClearRobotSettings.ResolutionHeight))
                        .AddParameter(nameof(ClearRobotSettings.LoginToConsole));
                    Invoke(cmdlet);
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathRobotSettings)
                        .AddParameter(UiPathStrings.Id, robotId);
                    var robotSettings = Invoke<RobotExecutionSettings>(cmdlet)?.FirstOrDefault();

                    Assert.IsNotNull(robotSettings);
                    Assert.IsNotNull(robotSettings.Id);
                    Assert.IsNull(robotSettings.ResolutionHeight);
                    Assert.IsNull(robotSettings.TracingLevel);
                    Assert.IsNull(robotSettings.LoginToConsole);
                    Assert.IsNull(robotSettings.StudioNotifyServer);
                    Assert.IsNull(robotSettings.FontSmoothing);
                    Assert.IsNull(robotSettings.ResolutionWidth);
                    Assert.IsNull(robotSettings.ResolutionDepth);
                }
            }
        }
    }
}
