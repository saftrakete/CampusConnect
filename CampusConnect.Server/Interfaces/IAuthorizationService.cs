using CampusConnect.Server.Models;
using CampusConnect.Server.Models.Dtos;

namespace CampusConnect.Server.Interfaces
{
    public interface IAuthorizationService
    {
        public bool Authorize(UserModel user, LoginDto loginDto);
        public string GenerateJwtToken(UserModel user);
        public string GenerateTempToken(UserModel user);
        public bool ValidateTempToken(string tempToken, out string loginName);
        public int GetUserIdFromToken(Microsoft.AspNetCore.Http.HttpContext context);
    }
}
