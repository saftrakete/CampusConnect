using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Server.Models
{
    [PrimaryKey("MessageId")]
    public partial class MessageModel
    {
        public string Content { get; set; }
        public int MessageId { get; set; }
    }
}
