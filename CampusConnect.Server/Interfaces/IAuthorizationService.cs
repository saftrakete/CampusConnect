using CampusConnect.Server.Models;

namespace CampusConnect.Server.Interfaces
{
    public interface IAuthorizationService
    {
        public string AuthorizeAndGetToken(UserModel user);
    }
}
