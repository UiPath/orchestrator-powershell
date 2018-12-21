using System;
using System.IO;
using System.Reflection;

namespace UiPath.PowerShell.Util
{
    internal static class BindingResolver
    {
        internal static bool Ignored = false;

        static BindingResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                if (args.Name.StartsWith("Newtonsoft.Json", StringComparison.InvariantCultureIgnoreCase))
                {
                    var path = Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        "Newtonsoft.Json.dll");
                    return Assembly.LoadFile(path);
                }
                else
                {
                    return null;
                }
            };
        }
    }
}
