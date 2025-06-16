using System.Collections.Generic;

namespace CampusConnect.Server.Models
{
    public partial class LearnGroup
    {
        public int LearnGroupId { get; set; }
        public string Name { get; set; }
        public virtual User Admin { get; set; }
        public virtual ICollection<User> Members { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<Module> ContainedModules { get; set; }
    }
}
