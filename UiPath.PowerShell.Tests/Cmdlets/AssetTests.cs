﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Tests.Util;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Tests
{
    [TestClass]
    public class AssetTests : TestClassBase
    {
        [TestMethod]
        public void AssetAddRemovePositional()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                Asset asset = null;

                // Positional add
                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    var assetName = TestRandom.RandomString();
                    var assetValue = TestRandom.RandomString();
                    cmdlet.AddCommand(UiPathStrings.AddUiPathAsset)
                        .AddArgument(assetName)
                        .AddParameter(UiPathStrings.TextValue, assetValue);
                    var assets = Invoke<Asset>(cmdlet);

                    Validators.ValidateAssetResponse(assets, null, assetName, AssetDtoValueType.Text, assetValue);

                    asset = assets[0];
                }

                // positional remove by object
                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.RemoveUiPathAsset)
                        .AddArgument(asset);
                    Invoke(cmdlet);
                }
            }
        }

        [TestMethod]
        public void AssetAddGetRemoveById()
        {
            using (var runspace = PowershellFactory.CreateAuthenticatedSession(TestContext))
            {
                var assetName = TestRandom.RandomString();
                var assetValue = TestRandom.RandomString();
                long? assetId = null;
                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.AddUiPathAsset)
                        .AddParameter(UiPathStrings.Name, assetName)
                        .AddParameter(UiPathStrings.TextValue, assetValue);
                    var assets = Invoke<Asset>(cmdlet);

                    Validators.ValidateAssetResponse(assets, null, assetName, AssetDtoValueType.Text, assetValue);

                    assetId = assets[0].Id;
                }

                // Get by name
                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathAsset)
                        .AddParameter(UiPathStrings.Name, assetName);
                    var assets = Invoke<Asset>(cmdlet);
                    Validators.ValidateAssetResponse(assets, assetId, assetName, AssetDtoValueType.Text, assetValue);
                }

                //Remove by Id
                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.RemoveUiPathAsset)
                        .AddParameter(UiPathStrings.Id, assetId);
                    Invoke(cmdlet);
                }

                // Validate is removed
                using (var cmdlet = PowershellFactory.CreateCmdlet(runspace))
                {
                    cmdlet.AddCommand(UiPathStrings.GetUiPathAsset)
                        .AddParameter(UiPathStrings.Name, assetName);
                    var assets = Invoke<Asset>(cmdlet);
                    Validators.ValidatEmptyResponse(assets);
                }
            }

        }
    }
}
