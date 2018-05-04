using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Tests.Util;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Tests.Cmdlets
{
    [TestClass]
    public class UserTests : TestClassBase
    {
        [TestMethod]
        public void UserAddGetRemove()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                User user = default(User);
                var userName = TestRandom.RandomAlphaNumeric();
                var password = TestRandom.RandomPassword();
                var name = TestRandom.RandomString();
                var surname = TestRandom.RandomString();
                var email = TestRandom.RandomEmail();
                UserDtoType userType = UserDtoType.User;

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.AddUiPathUser)
                        .AddParameter(UiPathStrings.Username, userName)
                        .AddParameter(UiPathStrings.Password, password)
                        .AddParameter(UiPathStrings.Name, name)
                        .AddParameter(UiPathStrings.Surname, surname)
                        .AddParameter(UiPathStrings.EmailAddress, email)
                        .AddParameter(UiPathStrings.Type, userType)
                        .AddParameter(UiPathStrings.RolesList, new List<string>() { UiPathStrings.Administrator });
                    var users = Invoke<User>(cmdlet);
                    Validators.ValidateUserResponse(users, null, userName, password, name, surname, email, userType);
                    user = users[0];
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathUser)
                        .AddParameter(UiPathStrings.Id, user.Id);
                    var users = Invoke<User>(cmdlet);
                    Validators.ValidateUserResponse(users, user.Id, userName, password, name, surname, email, userType);
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.RemoveUiPathUser)
                        .AddParameter(UiPathStrings.Id, user.Id);
                    Invoke(cmdlet);
                }

                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathUser)
                        .AddParameter(UiPathStrings.Username, userName);
                    var users = Invoke<User>(cmdlet);
                    Validators.ValidatEmptyResponse(users);
                }
            }
        }
    }
}
