using System.Collections.Generic;

namespace CampusConnect.Server.Models
{
    public partial class UserRole
    {
        public int UserRoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public virtual ICollection<string> Permissions { get; set; }
    }
}
