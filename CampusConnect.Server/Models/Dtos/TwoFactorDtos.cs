namespace CampusConnect.Server.Models.Dtos
{
    public class TwoFactorSetupDto
    {
        public string QrCodeUri { get; set; }
        public string ManualEntryKey { get; set; }
    }

    public class TwoFactorVerifyDto
    {
        public string LoginName { get; set; }
        public string Code { get; set; }
        public string? TempToken { get; set; }
    }

    public class TwoFactorDisableDto
    {
        public string Code { get; set; }
    }
}