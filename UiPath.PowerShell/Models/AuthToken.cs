using System;
using System.Management.Automation;

namespace UiPath.PowerShell.Models
{
    /// <summary>
    /// The Token needed to authenticate UiPath cmdlets
    /// </summary>
    public class AuthToken
    {
        public string URL { get; internal set; }

        [Hidden]
        public string Token { get; internal set; }

        public bool WindowsCredentials { get; internal set; }

        public bool Authenticated { get; internal set; }

        public Version ApiVersion { get; internal set; }

        public String BuildVersion { get; internal set; }

        public string OrganizationUnit { get; internal set; }

        public string TenantName { get; internal set; }

        internal long? OrganizationUnitId { get; set; }
    }
}
