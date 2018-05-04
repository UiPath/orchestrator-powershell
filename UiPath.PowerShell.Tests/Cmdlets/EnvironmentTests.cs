using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management.Automation;
using UiPath.PowerShell.Tests.Util;
using UiPath.Web.Client.Models;
using Environment = UiPath.PowerShell.Models.Environment;

namespace UiPath.PowerShell.Tests.Cmdlets
{
    [TestClass]
    public class EnvironmentTests : TestClassBase
    {
        [TestMethod]
        public void EnvironmentAddGetRemove()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                var name = TestRandom.RandomString();
                var description = TestRandom.RandomString();
                long? environmentId = null;
                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.AddUiPathEnvironment)
                        .AddParameter(UiPathStrings.Name, name)
                        .AddParameter(UiPathStrings.Description, description)
                        .AddParameter(UiPathStrings.Type, EnvironmentDtoType.Dev);
                    var environments = Invoke<Environment>(cmdlet);

                    Validators.ValidateEnvironmentResponse(environments, null, name, description, EnvironmentDtoType.Dev);

                    environmentId = environments[0].Id;
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathEnvironment)
                        .AddParameter(UiPathStrings.Id, environmentId);
                    var environments = Invoke<Environment>(cmdlet);

                    Validators.ValidateEnvironmentResponse(environments, environmentId, name, description, EnvironmentDtoType.Dev);
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.RemoveUiPathEnvironment)
                        .AddParameter(UiPathStrings.Id, environmentId);
                    Invoke(cmdlet);
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathEnvironment)
                        .AddParameter(UiPathStrings.Name, name);
                    var environments = Invoke<Environment>(cmdlet);
                    Validators.ValidatEmptyResponse(environments);
                }
            }
        }

        [TestMethod]
        public void EnvironmentAddType()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                foreach (var envType in Enum.GetValues(typeof(EnvironmentDtoType)))
                {
                    using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                    {
                        var name = TestRandom.RandomString();
                        var description = TestRandom.RandomString();
                        cmdlet.AddCommand(UiPathStrings.AddUiPathEnvironment)
                            .AddParameter(UiPathStrings.Name, name)
                            .AddParameter(UiPathStrings.Description, description)
                            .AddParameter(UiPathStrings.Type, envType);
                        var environments = Invoke<Environment>(cmdlet);

                        Validators.ValidateEnvironmentResponse(environments, null, name, description, (EnvironmentDtoType)envType);

                        Api.DeleteEnvironmentById(environments[0].Id);

                        TestContext.WriteLine($"Validated Add-UiPathEnvironment type: {envType}");
                    }
                }
            }
        }

        [TestMethod]
        public void EnvironmentNegativeBadType()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    var name = TestRandom.RandomString();
                    var description = TestRandom.RandomString();
                    cmdlet.AddCommand(UiPathStrings.AddUiPathEnvironment)
                        .AddParameter(UiPathStrings.Name, name)
                        .AddParameter(UiPathStrings.Description, description)
                        .AddParameter(UiPathStrings.Type, "Invalid");
                    // Not clear if it should be ParameterBindingValidationException or ValidationMetadataException
                    // And ParameterBindingValidationException does not resolve...
                    try
                    {
                        Invoke<Environment>(cmdlet);
                        Assert.Fail("The Invoke was supposed to throw!");
                    }
                    catch(RuntimeException re)
                    {
                        Assert.AreEqual("Cannot validate argument on parameter 'Type'. The argument \"Invalid\" does not belong to the set \"Dev,Test,Prod\" specified by the ValidateEnum attribute. Supply an argument that is in the set and then try the command again", re.Message);
                    }
                }
            }
        }
    }
}
