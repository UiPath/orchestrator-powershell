using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiPath.PowerShell.Tests.Util
{
    public class TestFileFixture: IDisposable
    {
        public string FileName { get; set; }

        public void Dispose()
        {
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }
        }
    }
}
