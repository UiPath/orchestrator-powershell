using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Windows.Forms;
using UiPath.PowerShell.Models;

namespace UiPath.PowerShell.OAuth
{
    internal partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public string AuthorizationCode { get; private set; }

        public string AuthorizationState { get; private set; }

        public string AuthorizationError { get; private set; }

        public string AuthorizationErrorDescription { get; private set; }

        public string AuthorizationErrorTracking { get; private set; }

        public AuthToken AuthToken { get; internal set; }

        public void LoginUser(bool usePrivate)
        {
            WinInetHelper.ResetCookiePersist();
            if (usePrivate)
            {
                WinInetHelper.SupressCookiePersist();
            }

            try
            {
                var verifier = GetRandomBase64UrlEncodedBytes(32);
                var challenge = GetBase64UrlEncodedBytes(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(verifier)));
                var nonce = GetRandomBase64UrlEncodedBytes(16);
                var state = GetRandomBase64UrlEncodedBytes(16);
                var client_id = AuthToken.ApplicationId;

                // The server validation code of the challenge and verifier is very strict
                // We must reproduce here all the quircks, so challenge and scope we encode ourselves
                //
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString["response_type"] = "code";
                queryString["nonce"] = nonce;
                queryString["state"] = state;
                queryString["code_challenge_method"] = "S256";
                queryString["audience"] = "https://orchestrator.cloud.uipath.com";
                queryString["client_id"] = client_id;
                queryString["redirect_uri"] = $"{AuthToken.AuthorizationUrl}/mobile";

                var url = $"{AuthToken.AuthorizationUrl}/authorize?code_challenge={challenge}&scope=openid+profile+offline_access+email&{queryString}";

                webBrowser.ScriptErrorsSuppressed = true;
                webBrowser.Navigate(url);

                if (DialogResult.OK == ShowDialog())
                {
                    AuthToken.AuthorizationCode = AuthorizationCode;
                    AuthToken.AuthorizationVerifier = verifier;
                    AuthToken.Authenticated = true;
                }
                else
                {
                    throw new ApplicationException($"Authorization failed: {AuthorizationError}: {AuthorizationErrorDescription}. Support tracking code: {AuthorizationErrorTracking}");
                }
            }
            finally
            {
                WinInetHelper.EndBrowserSession();
                WinInetHelper.ResetCookiePersist();
            }
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.ToString().StartsWith($"{AuthToken.AuthorizationUrl}/mobile", StringComparison.OrdinalIgnoreCase))
            {
                var query = HttpUtility.ParseQueryString(e.Url.Query);
                AuthorizationCode = query["code"];
                AuthorizationState = query["state"];

                DialogResult = DialogResult.OK;
                Close();
            }

            if (e.Url.PathAndQuery.StartsWith("/portal_/unhandlederror", StringComparison.OrdinalIgnoreCase))
            {
                var query = HttpUtility.ParseQueryString(e.Url.Query);
                AuthorizationError = query["error"];
                AuthorizationErrorDescription = query["error_description"];
                AuthorizationErrorTracking = query["tracking"];

                DialogResult = DialogResult.Abort;
                Close();
            }
        }

        internal static string GetRandomBase64UrlEncodedBytes(int length)
        {
            var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            return GetBase64UrlEncodedBytes(bytes);
        }

        internal static string GetBase64UrlEncodedBytes(byte[] bytes)
        {
            return Convert.ToBase64String(bytes).Replace("+", "-").Replace("=", String.Empty).Replace("/", "_");
        }

        private void WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            statusLoading.Text = e.Url.ToString();
        }

        private void WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            statusLoading.Text = e.Url.ToString();
        }
    }
}
