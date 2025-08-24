namespace CampusConnect.Server.Models.Dtos
{
    public class TwoFactorVerifyDto
    {
        public string LoginName { get; set; }
        public string Code { get; set; }
        public string? TempToken { get; set; }
    }
}