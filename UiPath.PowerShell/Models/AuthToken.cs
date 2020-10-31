using System;
using System.ComponentModel;
using System.Management.Automation;
using UiPath.PowerShell.Util;

namespace UiPath.PowerShell.Models
{
    /// <summary>
    /// The Token needed to authenticate UiPath cmdlets
    /// </summary>
    [TypeConverter(typeof(UiPathTypeConverter))]
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

        public Folder CurrentFolder { get; internal set; }

        public string TenantName { get; internal set; }

        public string AccountName { get; internal set; }

        public int? RequestTimeout { get; set; }

        public CloudDeployments CloudDeployment { get; set; }

        [Hidden]
        public string AuthorizationUrl
        {
            get => _authorizeUrl ?? GetDefaultAuthorizeUrl(CloudDeployment);
            internal set => _authorizeUrl = value;
        }

        internal long? OrganizationUnitId { get; set; }

        [Hidden]
        public string AuthorizationCode { get; internal set; }

        [Hidden]
        public string AuthorizationVerifier { get; internal set; }

        [Hidden]
        public string AuthorizationTokenId { get; internal set; }

        [Hidden]
        public string AuthorizationRefreshToken { get; internal set; }

        [Hidden]
        public string AccountUrl
        {
            get => _accountUrl ?? GetDefaultAccountUrl(CloudDeployment);
            internal set => _accountUrl = value;
        }

        [Hidden]
        public string ApplicationId
        {
            get => _applicationId ?? GetDefaultApplicationId(CloudDeployment);
            internal set => _applicationId = value;
        }

        private string _authorizeUrl;
        internal static string GetDefaultAuthorizeUrl(CloudDeployments cloudDeployment)
        {
            switch (cloudDeployment)
            {
                case CloudDeployments.Alpha:
                    return "https://id-alpha.uipath.com";
                case CloudDeployments.Staging:
                    return "https://id-preprod.uipath.com";
                case CloudDeployments.Production:
                    return "https://account.uipath.com";
                default:
                    return null;
            }
        }

        private string _accountUrl;

        internal static string GetDefaultAccountUrl(CloudDeployments cloudDeployment)
        {
            switch (cloudDeployment)
            {
                case CloudDeployments.Alpha:
                    return "https://alpha.uipath.com";
                case CloudDeployments.Staging:
                    return "https://staging.uipath.com";
                case CloudDeployments.Production:
                    return "https://cloud.uipath.com";
                default:
                    return null;
            }
        }

        private string _applicationId;

        internal static string GetDefaultApplicationId(CloudDeployments cloudDeployment)
        {
            switch (cloudDeployment)
            {
                case CloudDeployments.Alpha:
                    return "H12WyzXvethJ1w4o60MxwMk8GERuJQ6D";
                case CloudDeployments.Staging:
                    return "hDopoDWPKEMF3eTc6YexL5ePqpfSoc9Z";
                case CloudDeployments.Production:
                    return "qccWcbcRgHGP0Nivlm7T9B7lven0AnTW";
                default:
                    return null;
            }
        }




    }
}
