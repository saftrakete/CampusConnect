using CampusConnect.Server.Enums;
using Microsoft.Identity.Client;

namespace CampusConnect.Server.Models
{
    public partial class Module
    {
        public Module() { }

        public Module(string name, Faculty faculty, DifficultyEnum difficulty)
        {
            Name = name;
            Faculty = faculty;
            Difficulty = difficulty;
        }

        public int ModuleId { get; set; }
        public string Name { get; set; }
        public Faculty Faculty { get; set; }
        public DifficultyEnum Difficulty { get; set; }
    }
}
