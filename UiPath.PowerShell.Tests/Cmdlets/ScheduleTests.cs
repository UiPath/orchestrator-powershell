using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Tests.Util;

namespace UiPath.PowerShell.Tests.Cmdlets
{
    [TestClass]
    public class ScheduleTests : TestClassBase
    {
        public const string TestCron = "0 30 11 1/1 * ? *";

        [TestMethod]
        public void ScheduleAddRemove()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                ProcessSchedule schedule = null;
                Process process = null;
                var name = TestRandom.RandomString();
                var description = TestRandom.RandomString();

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathProcess)
                        .AddParameter(UiPathStrings.Id, 2);
                    var processes = Invoke<Process>(cmdlet);
                    process = processes[0];
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.AddUiPathProcessSchedule)
                        .AddParameter(UiPathStrings.Name, name)
                        .AddParameter(UiPathStrings.Process, process)
                        .AddParameter(UiPathStrings.StartProcessCron, TestCron)
                        .AddParameter(UiPathStrings.TimeZoneId, TimeZone.CurrentTimeZone.StandardName)
                        .AddParameter(UiPathStrings.RobotCount, 1);
                    var schedules = Invoke<ProcessSchedule>(cmdlet);

                    schedule = schedules[0];
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.RemoveUiPathProcessSchedule)
                        .AddParameter(UiPathStrings.Id, schedule.Id);
                    Invoke(cmdlet);
                }
            }
        }
    }
}
