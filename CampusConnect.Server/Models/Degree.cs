using System.Collections;
using System.Collections.Generic;

namespace CampusConnect.Server.Models
{
    public partial class Degree
    {
        public int DegreeId { get; set; }
        public string Name { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual ICollection<Module> MandatoryModules { get; set; }
    }
}
