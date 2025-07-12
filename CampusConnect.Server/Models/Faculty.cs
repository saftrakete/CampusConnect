using System.Collections.Generic;

namespace CampusConnect.Server.Models
{
    public partial class Faculty
    {
        public Faculty() { }

        public Faculty(string name, string facultyCode, List<Degree> degrees, List<Module> modules)
        {
            Name = name;
            FacultyCode = facultyCode;
            Degrees = degrees ?? new List<Degree>();
            Modules = modules ?? new List<Module>();
        }

        public int FacultyId { get; set; }
        public string Name { get; set; }
        public string FacultyCode { get; set; } // Assuming this is a code or abbreviation for the faculty
        public virtual ICollection<Degree> Degrees { get; set; } = new List<Degree>();
        public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
    }
}
