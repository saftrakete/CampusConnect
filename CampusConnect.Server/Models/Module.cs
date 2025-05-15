using CampusConnect.Server.Enums;

namespace CampusConnect.Server.Models
{
    public partial class Module
    {
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public int Semester { get; set; }

        public Faculty Faculty { get; set; }
        public DifficultyEnum Difficulty { get; set; }
    }
}
