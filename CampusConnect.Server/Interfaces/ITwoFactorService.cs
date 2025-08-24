namespace CampusConnect.Server.Interfaces
{
    public interface ITwoFactorService
    {
        string GenerateSecret();
        string GenerateQrCodeUri(string secret, string userEmail, string issuer = "CampusConnect");
        string GenerateQrCodeImage(string uri);
        bool ValidateCode(string secret, string code);
    }
}