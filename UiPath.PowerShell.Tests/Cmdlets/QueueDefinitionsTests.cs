using Microsoft.VisualStudio.TestTools.UnitTesting;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Tests.Util;

namespace UiPath.PowerShell.Tests.Cmdlets
{
    [TestClass]
    public class QueueDefinitionsTests: TestClassBase
    {
        [TestMethod]
        public void QueueDefinitionAddGetRemove()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                QueueDefinition queue = null;
                var name = TestRandom.RandomString();
                var description = TestRandom.RandomString();

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.AddUiPathQueueDefinition)
                        .AddParameter(UiPathStrings.Name, name)
                        .AddParameter(UiPathStrings.Description, description)
                        .AddParameter(UiPathStrings.AcceptAutomaticallyRetry);
                    var queues = cmdlet.Invoke<QueueDefinition>();

                    Validators.ValidateQueueDefinitionResponse(queues, null, name, description, true, false, 0);

                    queue = queues[0];
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathQueueDefinition)
                        .AddParameter(UiPathStrings.Id, queue.Id);
                    var queues = cmdlet.Invoke<QueueDefinition>();

                    Validators.ValidateQueueDefinitionResponse(queues, queue.Id, name, description, true, false, 0);
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.RemoveUiPathQueueDefinition)
                        .AddParameter(UiPathStrings.Id, queue.Id);
                    cmdlet.Invoke();
                }
            }
        }

        [TestMethod]
        public void QueueDefinitionAddEditGetRemove()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                QueueDefinition queue = null;
                var name = TestRandom.RandomString();
                var description = TestRandom.RandomString();

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.AddUiPathQueueDefinition)
                        .AddParameter(UiPathStrings.Name, name)
                        .AddParameter(UiPathStrings.Description, description)
                        .AddParameter(UiPathStrings.AcceptAutomaticallyRetry);
                    var queues = cmdlet.Invoke<QueueDefinition>();

                    Validators.ValidateQueueDefinitionResponse(queues, null, name, description, true, false, 0);

                    queue = queues[0];
                }

                // Positional Get
                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    description = TestRandom.RandomString();
                    cmdlet.AddCommand(UiPathStrings.EditUiPathQueueDefinition)
                        .AddArgument(queue)
                        .AddParameter(UiPathStrings.Description, description);
                    cmdlet.Invoke<QueueDefinition>();
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathQueueDefinition)
                        .AddParameter(UiPathStrings.Id, queue.Id);
                    var queues = cmdlet.Invoke<QueueDefinition>();

                    Validators.ValidateQueueDefinitionResponse(queues, queue.Id, name, description, true, false, 0);
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.RemoveUiPathQueueDefinition)
                        .AddParameter(UiPathStrings.QueueDefinition, queue);
                    cmdlet.Invoke();
                }
            }
        }
    }
}
