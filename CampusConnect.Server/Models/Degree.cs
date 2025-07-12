using System.Collections.Generic;

namespace CampusConnect.Server.Models
{
    public partial class Degree
    {
        public Degree() { }

        public Degree(string name, List<Module> mandatoryModules)
        {
            Name = name;
            MandatoryModules = mandatoryModules ?? new List<Module>();
        }

        public int DegreeId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Module> MandatoryModules { get; set; }
    }
}
