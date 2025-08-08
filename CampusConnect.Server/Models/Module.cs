using CampusConnect.Server.Enums;
using System.Collections.Generic;

namespace CampusConnect.Server.Models
{
    public partial class Module
    {
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public int Semester { get; set; }

        public Faculty Faculty { get; set; }
        public DifficultyEnum Difficulty { get; set; }
        public virtual ICollection<Degree> CorrespondingDegrees { get; set; }
        public virtual ICollection<UserModel> Attendees { get; set; }

    }
}
