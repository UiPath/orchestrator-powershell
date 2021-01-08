using System;

namespace UiPath.PowerShell.Util
{
    internal static class OrchestratorProtocolVersion
    {
        internal static  Version V18_1 => Version.Parse("4.0");

        internal static Version V18_2 => Version.Parse("5.0");

        internal static  Version V18_3 => Version.Parse("6.0");

        internal static Version V18_4 => Version.Parse("7.0");

        internal static Version V19_10 => Version.Parse("9.0");

        internal static Version V20_4 => Version.Parse("10.0");
    }
}
