using System;

namespace UiPath.PowerShell.Util
{
    internal static class OrchestratorProtocolVersion
    {
        internal static  Version V18_1 => Version.Parse("5.0");
        internal static  Version V18_3 => Version.Parse("6.0");
    }
}
