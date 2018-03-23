using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UiPath.PowerShell
{
    internal static class BindingResolver
    {
        internal static bool Ignored = false;

        static BindingResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                if (args.Name.Equals("Newtonsoft.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed", StringComparison.InvariantCultureIgnoreCase))
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
