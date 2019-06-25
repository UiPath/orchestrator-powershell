using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UiPath.PowerShell.Tests.Util
{
    internal static class TestContextExtensions
    {
        internal static string GetTestParameter(this TestContext testContext, string name)
        {
            var propValue = testContext.Properties[name]?.ToString();
            if (string.IsNullOrWhiteSpace(propValue))
            {
                throw new ArgumentException($"Ensure that the \"{name}\" parameter is set in the .runsettings file, and that the .runsettings file has been attached to the test runner.", name);
            }

            return propValue;
        }
    }
}
