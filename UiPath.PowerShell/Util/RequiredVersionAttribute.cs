using System;

namespace UiPath.PowerShell.Util
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredVersionAttribute : Attribute
    {
        public string MinVersion { get; set; }
    }
}
