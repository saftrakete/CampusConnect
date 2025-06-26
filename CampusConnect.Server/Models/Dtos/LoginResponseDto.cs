namespace CampusConnect.Server.Models.Dtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }
        public bool RequiresTwoFactor { get; set; } = false;
        public string? TempToken { get; set; }
    }
}
