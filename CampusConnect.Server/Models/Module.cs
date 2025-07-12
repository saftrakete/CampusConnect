using CampusConnect.Server.Enums;
using System.Collections.Generic;

namespace CampusConnect.Server.Models
{
    public partial class Module
    {
        public Module() { }

        public Module(string name, string description, DifficultyEnum difficulty)
        {
            Name = name;
            Difficulty = difficulty;
            Description = description;
        }

        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DifficultyEnum Difficulty { get; set; }
    }
}
