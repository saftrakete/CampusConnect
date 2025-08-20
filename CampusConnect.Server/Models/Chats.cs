using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Server.Models
{
    [PrimaryKey("ChatId")]
    public class Chats
    {
        public int ChatId { get; set; }
        public string ChatName { get; set; }
        public string ChatAdmin { get; set; }
    }
}
