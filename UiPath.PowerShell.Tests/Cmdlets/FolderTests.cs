using Microsoft.VisualStudio.TestTools.UnitTesting;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Tests.Util;
using UiPath.Web.Client201910.Models;

namespace UiPath.PowerShell.Tests.Cmdlets
{
    [TestClass]
    public class FolderTests : TestClassBase
    {
        [TestMethod]
        public void FolderAddGetRemove()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                var displayName = TestRandom.RandomAlphaNumeric();
                var description = TestRandom.RandomAlphaNumeric();
                long? folderId = null;
                Folder currentFolder = null;
                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.AddFolder)
                        .AddParameter(UiPathStrings.DisplayName, displayName)
                        .AddParameter(UiPathStrings.Description, description)
                        .AddParameter(UiPathStrings.PermissionModel, FolderDtoPermissionModel.FineGrained)
                        .AddParameter(UiPathStrings.ProvisionType, FolderDtoProvisionType.Automatic);
                    var folders = Invoke<Folder>(cmdlet);

                    Validators.ValidateFolderResponse(folders, null, displayName, description, FolderDtoProvisionType.Automatic, FolderDtoPermissionModel.FineGrained);

                    folderId = folders[0].Id;
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetFolder)
                        .AddParameter(UiPathStrings.Id, folderId);
                    var folders = Invoke<Folder>(cmdlet);
                    currentFolder = folders[0];

                    Validators.ValidateFolderResponse(folders, folderId, displayName, description, FolderDtoProvisionType.Automatic, FolderDtoPermissionModel.FineGrained);
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    var updatedDisplayName = TestRandom.RandomString();
                    cmdlet.AddCommand(UiPathStrings.EditFolder)
                        .AddArgument(currentFolder)
                        .AddParameter(UiPathStrings.DisplayName, updatedDisplayName);
                    Invoke<Folder>(cmdlet);
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.RemoveFolder)
                        .AddParameter(UiPathStrings.Id, folderId);
                    Invoke(cmdlet);
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetFolder)
                        .AddParameter(UiPathStrings.DisplayName, displayName);
                    var folders = Invoke<Folder>(cmdlet);
                    Validators.ValidatEmptyResponse(folders);
                }
            }
        }

        [TestMethod]
        public void CurrentUserFolders()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                var displayName = TestRandom.RandomAlphaNumeric();
                var description = TestRandom.RandomAlphaNumeric();
                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetCurrentUserFolders);
                    var folders = Invoke<ExtendedFolder>(cmdlet);

                    Assert.IsNotNull(folders);
                }
            }
        }

        [TestMethod]
        public void FolderUsers()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                var displayName = TestRandom.RandomAlphaNumeric();
                var description = TestRandom.RandomAlphaNumeric();
                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetFolderUsers)
                         .AddParameter(UiPathStrings.Id, 1);
                    var folders = Invoke<UserRole>(cmdlet);

                    Assert.IsNotNull(folders);

                }
            }
        }

        [TestMethod]
        public void Folders()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                var displayName = TestRandom.RandomAlphaNumeric();
                var description = TestRandom.RandomAlphaNumeric();
                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetFolder);
                    var folders = Invoke<Folder>(cmdlet);

                    Assert.IsNotNull(folders);

                }
            }
        }
    }
}
