using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Management.Automation.Runspaces;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Tests.Util;
using Cmdlet = System.Management.Automation.PowerShell;

namespace UiPath.PowerShell.Tests.Cmdlets
{
    [TestClass]
    public class LibraryTests : TestClassBase
    {
        [TestMethod]
        public void LibraryAddRemovePositional()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                Library library = null;

                var libSpec = TestRandom.RandomPakcageSpec();

                using (var testPackage = TestRandom.RandomPackage(libSpec))
                {

                    // Positional add
                    using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                    {
                        cmdlet.AddCommand(UiPathStrings.AddUiPathLibrary)
                            .AddArgument(testPackage.FileName);
                        Invoke(cmdlet);
                    }
                }

                // Get by Id
                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathLibrary)
                            .AddParameter(UiPathStrings.Id, libSpec.Id);

                    var libraries = Invoke<Library>(cmdlet);

                    Validators.ValidateLibraryResponse(libraries, libSpec);

                    library = libraries[0];

                    Assert.IsTrue(library.IsLatestVersion.HasValue);
                    Assert.IsTrue(library.IsLatestVersion.Value);
                }

                // Only Host admin can delete Libraries
                //
                using (var hostRunspace = PowershellFactory.CreateHostAuthenticatedSession(TestContext))
                {

                    // positional remove by object
                    using (var cmdlet = PowershellFactory.CreateCmdlet(hostRunspace))
                    {
                        cmdlet.AddCommand(UiPathStrings.RemoveUiPathLibrary)
                            .AddArgument(library);
                        Invoke(cmdlet);
                    }
                }
            }
        }


        [TestMethod]
        public void LibraryAddVersion()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                Library libV1 = null;
                Library libV2 = null;

                var libSpecV1 = TestRandom.RandomPakcageSpec();

                using (var testPackage = TestRandom.RandomPackage(libSpecV1))
                {
                    using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                    {
                        cmdlet.AddCommand(UiPathStrings.AddUiPathLibrary)
                            .AddArgument(testPackage.FileName);
                        Invoke(cmdlet);
                    }
                }

                var libSpecV2 = new PackageSpec
                {
                    Id = libSpecV1.Id,
                    Title = libSpecV1.Title,
                    Authors = libSpecV1.Authors,
                    Description = libSpecV1.Description,
                    ReleaseNotes = TestRandom.RandomText(15, 25),
                    Version = new Version(libSpecV1.Version.Major + 1, 0, 1)
                };


                using (var testPackage = TestRandom.RandomPackage(libSpecV2))
                {
                    using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                    {
                        cmdlet.AddCommand(UiPathStrings.AddUiPathLibrary)
                            .AddArgument(testPackage.FileName);
                        Invoke(cmdlet);
                    }
                }

                // Get all versions
                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathLibraryVersion)
                            .AddParameter(UiPathStrings.Id, libSpecV1.Id);

                    var libraries = Invoke<Library>(cmdlet);

                    Validators.ValidateLibraryResponse(libraries, libSpecV2, libSpecV1);

                    libV1 = libraries[0];

                    Assert.IsTrue(libV1.IsLatestVersion.HasValue);
                    Assert.IsTrue(libV1.IsLatestVersion.Value);

                    libV2 = libraries[1];

                    Assert.IsTrue(libV2.IsLatestVersion.HasValue);
                    Assert.IsFalse(libV2.IsLatestVersion.Value);

                }


                // Only Host admin can delete Libraries
                //
                using (var hostRunspace = PowershellFactory.CreateHostAuthenticatedSession(TestContext))
                {

                    // Remove by Id, will remove all versions
                    using (var cmdlet = PowershellFactory.CreateCmdlet(hostRunspace))
                    {
                        cmdlet.AddCommand(UiPathStrings.RemoveUiPathLibrary)
                            .AddParameter(UiPathStrings.Id, libV1.Id);
                        Invoke(cmdlet);
                    }
                }
            }
        }

        private void TestFilter(Runspace session, Action<Cmdlet> action, PackageSpec expected)
        {
            using (var cmdlet = PowershellFactory.CreateCmdlet(session))
            {
                cmdlet.AddCommand(UiPathStrings.GetUiPathLibrary);
                action(cmdlet);

                var libraries = Invoke<Library>(cmdlet);

                Validators.ValidateLibraryResponse(libraries, expected);
            }
        }


        [TestMethod]
        public void LibraryFind()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                var libSpec1 = TestRandom.RandomPakcageSpec();

                using (var testPackage = TestRandom.RandomPackage(libSpec1))
                {
                    using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                    {
                        cmdlet.AddCommand(UiPathStrings.AddUiPathLibrary)
                            .AddArgument(testPackage.FileName);
                        Invoke(cmdlet);
                    }
                }

                var libSpec2 = TestRandom.RandomPakcageSpec();


                using (var testPackage = TestRandom.RandomPackage(libSpec2))
                {
                    using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                    {
                        cmdlet.AddCommand(UiPathStrings.AddUiPathLibrary)
                            .AddArgument(testPackage.FileName);
                        Invoke(cmdlet);
                    }
                }

                TestFilter(runspace, (cmdlet) => cmdlet.AddParameter(UiPathStrings.Id, libSpec1.Id), libSpec1);
                TestFilter(runspace, (cmdlet) => cmdlet.AddParameter(UiPathStrings.Id, libSpec2.Id), libSpec2);
                TestFilter(runspace, (cmdlet) => cmdlet.AddParameter(UiPathStrings.Title, libSpec1.Title), libSpec1);
                TestFilter(runspace, (cmdlet) => cmdlet.AddParameter(UiPathStrings.Authors, libSpec1.Authors), libSpec1);
                TestFilter(runspace, (cmdlet) => cmdlet.AddParameter(UiPathStrings.Version, libSpec2.Version), libSpec2);

                // Only Host admin can delete Libraries
                //
                using (var hostRunspace = PowershellFactory.CreateHostAuthenticatedSession(TestContext))
                {

                    // Remove 
                    using (var cmdlet = PowershellFactory.CreateCmdlet(hostRunspace))
                    {
                        cmdlet.AddCommand(UiPathStrings.RemoveUiPathLibrary)
                            .AddParameter(UiPathStrings.Id, libSpec1.Id);
                        Invoke(cmdlet);
                    }

                    using (var cmdlet = PowershellFactory.CreateCmdlet(hostRunspace))
                    {
                        cmdlet.AddCommand(UiPathStrings.RemoveUiPathLibrary)
                            .AddParameter(UiPathStrings.Id, libSpec2.Id);
                        Invoke(cmdlet);
                    }
                }
            }
        }
    }
}

