using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Server.Models
{
    [PrimaryKey("UserId")]
    public partial class UserModel
    {
        public int UserId { get; set; }
        public string Nickname { get; set; }
        public string LoginName { get; set; }
        public string PasswordHash { get; set; }
    }
}
