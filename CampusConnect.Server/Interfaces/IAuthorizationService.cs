using CampusConnect.Server.Models;
using CampusConnect.Server.Models.Dtos;

namespace CampusConnect.Server.Interfaces
{
    public interface IAuthorizationService
    {
        public bool Authorize(UserModel user, LoginDto loginDto);
        public string GenerateJwtToken(UserModel user);
    }
}
