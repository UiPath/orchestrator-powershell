﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using UiPath.PowerShell.Models;
using UiPath.Web.Client20194.Models;
using Environment = UiPath.PowerShell.Models.Environment;

namespace UiPath.PowerShell.Tests.Util
{
    internal static class Validators
    {
        public static void ValidateLibraryResponse(ICollection<Library> libraries, params PackageSpec[] expectedSpecs)
        {
            Assert.IsNotNull(libraries);
            Assert.AreEqual(expectedSpecs.Length, libraries.Count);

            int idx = 0;
            foreach (var l in libraries)
            {
                ValidateLibraryResponse(l, expectedSpecs[idx]);
                ++idx;
            }
        }

        public static void ValidateLibraryResponse(Library library, PackageSpec expectedSpec)
        {
            Assert.AreEqual(expectedSpec.Id, library.Id);
            Assert.AreEqual(expectedSpec.Version.ToString(), library.Version);
            Assert.AreEqual(expectedSpec.Authors, library.Authors);
            Assert.AreEqual(expectedSpec.Title, library.Title);
            Assert.AreEqual(expectedSpec.Description, library.Description);

            // CRLF is mangled in the nuspec
            var cmp = CompareInfo.GetCompareInfo("en-US");
            Assert.AreEqual(0, cmp.Compare(expectedSpec.ReleaseNotes, library.ReleaseNotes, CompareOptions.IgnoreSymbols),
                $"Release notes (ignoring symbols) don't match: {expectedSpec.ReleaseNotes} vs: {library.ReleaseNotes}");
        }

        public static void ValidateAssetResponse(ICollection<Asset> assets, long? expectedId, string expectedName, AssetDtoValueType expectedType, string expectedValue)
        {
            Assert.IsNotNull(assets);
            Assert.AreEqual(1, assets.Count);

            var asset = assets.First();

            Assert.AreNotEqual(0, asset.Id);
            if (expectedId.HasValue)
            {
                Assert.AreEqual(expectedId.Value, asset.Id);
            }
            Assert.AreEqual(expectedName, asset.Name);
            Assert.AreEqual(expectedType, asset.ValueType);
            Assert.AreEqual(expectedValue, asset.Value);
        }


        public static void ValidateEnvironmentResponse(ICollection<Environment> environments, long? expectedId, string expectedName, string expectedDescription, EnvironmentDtoType expectedType)
        {
            Assert.IsNotNull(environments);
            Assert.AreEqual(1, environments.Count);

            var environment = environments.First();

            Assert.AreNotEqual(0, environment.Id);
            if (expectedId.HasValue)
            {
                Assert.AreEqual(expectedId.Value, environment.Id);
            }
            Assert.AreEqual(expectedName, environment.Name);
            Assert.AreEqual(expectedType, environment.Type);
            Assert.AreEqual(expectedDescription, environment.Description);
        }

        internal static void ValidateRoleResult(Collection<Role> roles, int? id, string name, string displaName, bool isEditable, bool isStatic, List<string> permissions)
        {
            Assert.IsNotNull(roles);
            Assert.AreEqual(1, roles.Count);

            var role = roles.First();

            Assert.AreNotEqual(0, role.Id);
            if (id.HasValue)
            {
                Assert.AreEqual(id, role.Id);
            }
            Assert.AreEqual(name, role.Name);
            Assert.AreEqual(displaName, role.DisplayName);
            Assert.AreEqual(isEditable, role.IsEditable);
            Assert.AreEqual(isStatic, role.IsStatic);


            if (permissions != null)
            {
                Assert.IsTrue(Enumerable.SequenceEqual(permissions, role.Permissions), $"The expected permissions:'{string.Join(",", permissions)}' does not match the role permissions:'{string.Join(",", role.Permissions)}' ");
            }
        }

        internal static void ValidateUserResponse(ICollection<User> users, long? expectedId, string userName, string password, string name, string surname, string email, UserDtoType type)
        {
            Assert.IsNotNull(users);
            Assert.AreEqual(1, users.Count);

            var user = users.First();

            Assert.AreNotEqual(0, user.Id);
            if (expectedId.HasValue)
            {
                Assert.AreEqual(expectedId, user.Id);
            }

            Assert.AreEqual(userName, user.UserName);
            Assert.AreEqual(name, user.Name);
            Assert.AreEqual(surname, user.Surname);
            Assert.AreEqual(email, user.EmailAddress);
            Assert.AreEqual(type, user.Type);
        }

        internal static void ValidateQueueDefinitionResponse(ICollection<QueueDefinition> queues, long? expectedId, string expectedName, string expectedDescription, bool expectedAutoRetry, bool expectedEnforceUniq, int expectedMaxRetry)
        {
            Assert.IsNotNull(queues);
            Assert.AreEqual(1, queues.Count);

            var queue = queues.First();

            Assert.AreNotEqual(0, queue.Id);
            if (expectedId.HasValue)
            {
                Assert.AreEqual(expectedId.Value, queue.Id);
            }
            Assert.AreEqual(expectedName, queue.Name);
            Assert.AreEqual(expectedDescription, queue.Description);
            Assert.IsTrue(queue.AcceptAutomaticallyRetry.HasValue);
            Assert.AreEqual(expectedAutoRetry, queue.AcceptAutomaticallyRetry.Value);
            Assert.IsTrue(queue.EnforceUniqueReference.HasValue);
            Assert.AreEqual(expectedEnforceUniq, queue.EnforceUniqueReference.Value);
            Assert.IsTrue(queue.MaxNumberOfRetries.HasValue);
            Assert.AreEqual(expectedMaxRetry, queue.MaxNumberOfRetries);
        }

        internal static void ValidateRobotResponse(ICollection<Robot> robots, long? expectedId, string expectedName, string expectedDescription, string expectedMachine, Guid expectedLicenseKey, RobotDtoType expectedType)
        {
            Assert.IsNotNull(robots);
            Assert.AreEqual(1, robots.Count);

            var robot = robots.First();

            Assert.AreNotEqual(0, robot.Id);
            if (expectedId.HasValue)
            {
                Assert.AreEqual(expectedId, robot.Id);
            }

            Assert.AreEqual(expectedName, robot.Name);
            Assert.AreEqual(expectedDescription, robot.Description);
            Assert.AreEqual(expectedMachine, robot.MachineName);
            Assert.AreEqual(expectedType.ToString(), robot.Type);
            Assert.AreEqual(expectedLicenseKey.ToString(), robot.LicenseKey);
        }

        internal static void ValidatEmptyResponse<T>(ICollection<T> response)
        {
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Any());
        }

        internal static void ValidateFolderResponse(ICollection<Folder> folders, long? expectedId, string expectedDisplayName, string expectedDescription, UiPath.Web.Client201910.Models.FolderDtoProvisionType provisionType, UiPath.Web.Client201910.Models.FolderDtoPermissionModel permissionModel)
        {
            Assert.IsNotNull(folders);
            Assert.AreEqual(1, folders.Count);

            var folder = folders.First();

            Assert.AreNotEqual(0, folder.Id);
            if (expectedId.HasValue)
            {
                Assert.AreEqual(expectedId.Value, folder.Id);
            }
            Assert.AreEqual(expectedDisplayName, folder.DisplayName);
            Assert.AreEqual(expectedDescription, folder.Description);
            Assert.AreEqual(provisionType, folder.ProvisionType);
            Assert.AreEqual(permissionModel, folder.PermissionModel);
        }
    }
}
