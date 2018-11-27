using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiPath.PowerShell.Tests.Util
{
    public class PackageSpec
    {
        public string Id { get; set; }
        public Version Version { get; set; }
        public string Authors { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReleaseNotes { get; set; }
    }
}
