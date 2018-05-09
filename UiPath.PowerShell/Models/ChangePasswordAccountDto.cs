namespace UiPath.PowerShell.Models
{
    internal class ChangePasswordAccountDto
    {
        public long UserId { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
