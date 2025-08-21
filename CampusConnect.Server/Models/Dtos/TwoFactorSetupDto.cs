namespace CampusConnect.Server.Models.Dtos
{
    public class TwoFactorSetupDto
    {
        public string QrCodeUri { get; set; }
        public string ManualEntryKey { get; set; }
    }
}