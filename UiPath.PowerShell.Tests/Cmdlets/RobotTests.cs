using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Tests.Util;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Tests.Cmdlets
{
    [TestClass]
    public class RobotTests : TestClassBase
    {
        [TestMethod]
        public void RobotAddGetRemove()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                var name = TestRandom.RandomString();
                var description = TestRandom.RandomString();
                var licenseKey = Guid.NewGuid();
                var machine = TestRandom.RandomString();
                var username = TestRandom.RandomString();
                long? robotId = null;

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.AddUiPathRobot)
                        .AddParameter(UiPathStrings.Name, name)
                        .AddParameter(UiPathStrings.MachineName, machine)
                        .AddParameter(UiPathStrings.Description, description)
                        .AddParameter(UiPathStrings.LicenseKey, licenseKey)
                        .AddParameter(UiPathStrings.Username, username)
                        .AddParameter(UiPathStrings.Type, RobotDtoType.NonProduction);
                    var robots = cmdlet.Invoke<Robot>();

                    Validators.ValidateRobotResponse(robots, null, name, description, machine, licenseKey, RobotDtoType.NonProduction);

                    robotId = robots[0].Id;
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathRobot)
                        .AddParameter(UiPathStrings.Id, robotId);
                    var robots = cmdlet.Invoke<Robot>();

                    Validators.ValidateRobotResponse(robots, robotId, name, description, machine, licenseKey, RobotDtoType.NonProduction);
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.RemoveUiPathRobot)
                        .AddParameter(UiPathStrings.Id, robotId);
                    cmdlet.Invoke<Robot>();
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathRobot)
                        .AddParameter(UiPathStrings.Name, name);
                    var robots = cmdlet.Invoke<Robot>();

                    Validators.ValidatEmptyResponse(robots);
                }
            }
        }


        [TestMethod]
        public void RobotAddEditGetRemovePositional()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                var name = TestRandom.RandomString();
                var description = TestRandom.RandomString();
                var licenseKey = Guid.NewGuid();
                var machine = TestRandom.RandomString();
                var username = TestRandom.RandomString();
                long? robotId = null;

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.AddUiPathRobot)
                        .AddParameter(UiPathStrings.Name, name)
                        .AddParameter(UiPathStrings.MachineName, machine)
                        .AddParameter(UiPathStrings.Description, description)
                        .AddParameter(UiPathStrings.LicenseKey, licenseKey)
                        .AddParameter(UiPathStrings.Username, username)
                        .AddParameter(UiPathStrings.Type, RobotDtoType.NonProduction);
                    var robots = cmdlet.Invoke<Robot>();

                    Validators.ValidateRobotResponse(robots, null, name, description, machine, licenseKey, RobotDtoType.NonProduction);

                    robotId = robots[0].Id;
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.EditUiPathRobot)
                        .AddParameter(UiPathStrings.Id, robotId)
                        .AddParameter(UiPathStrings.Password, TestRandom.RandomPassword())
                        .AddParameter(UiPathStrings.Type, RobotDtoType.Unattended);
                    cmdlet.Invoke<Robot>();
                }

                Robot robot = null;

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathRobot)
                        .AddParameter(UiPathStrings.Id, robotId);
                    var robots = cmdlet.Invoke<Robot>();

                    Validators.ValidateRobotResponse(robots, robotId, name, description, machine, licenseKey, RobotDtoType.Unattended);
                    robot = robots[0];
                }

                // Positional object Robot argument to Remove
                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.RemoveUiPathRobot)
                        .AddArgument(robot);
                    cmdlet.Invoke<Robot>();
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathRobot)
                        .AddParameter(UiPathStrings.Name, name);
                    var robots = cmdlet.Invoke<Robot>();

                    Validators.ValidatEmptyResponse(robots);
                }
            }
        }
    }
}
