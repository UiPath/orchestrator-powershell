namespace UiPath.PowerShell.Models
{
    /// <summary>
    /// The Token needed to authenticate UiPath cmdlets
    /// </summary>
    public class AuthToken
    {
        public string URL { get; internal set; }

        public string Token { get; internal set; }

        public bool WindowsCredentials { get; internal set; }
    }
}
