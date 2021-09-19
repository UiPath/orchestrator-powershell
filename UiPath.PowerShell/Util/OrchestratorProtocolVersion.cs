using System;

namespace UiPath.PowerShell.Util
{
    internal static class OrchestratorProtocolVersion
    {
        internal const string sV19_10 = "9.0";
        internal const string sV20_4 = "10.0";
        internal const string sV18_1 = "4.0";

        internal static  Version V18_1 => Version.Parse(sV18_1);

        internal static Version V19_10 => Version.Parse(sV19_10);

        internal static Version V20_4 => Version.Parse(sV20_4);
    }
}
